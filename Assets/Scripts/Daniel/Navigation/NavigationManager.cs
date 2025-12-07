using UnityEngine;
using System.Collections.Generic;
using Input;
using Taxes;
//C 2025 Daniel Snapir alias Baltazar Benoni


namespace Navigation
{
public class NavigationManager : MonoBehaviour
{
        //
        //List to store many customers and their prices if needed.
        List<Customer> customers = new();

        //Mittarin tila.
        [SerializeField] State status;
        [SerializeField] PaymentView paymentView;
        public State Status { get { return status; } }

        #region Canvases
        [Header("Eri näkymät")]
        //Canvases to activate or deactivate according to state.
        [Header("Taksavalintasivut")]
        [SerializeField] GameObject taxCanvasOnMove;
        [SerializeField] GameObject taxCanvasPrelim;
        [Header("Valinnat-sivut")]
        [SerializeField] GameObject optionsCanvasOnComission;
        [SerializeField] GameObject optionsCanvasCheckOut;
        [SerializeField] GameObject optionsCanvasFree;
        [Header("Vapaatila")]
        [SerializeField] GameObject freeStateCanvas;
        [Header("Kassa")]
        [SerializeField] GameObject checkOutGeneralCanvas;
        [Header("Maksu-näkymät")]
        [SerializeField] GameObject paymentCanvas;
        [SerializeField] GameObject paymentCanvasSecondary;
        [SerializeField] GameObject paymentExecutionCanvas;

        [Header("Ajo-näkymä")]
        [SerializeField] GameObject onComissionCanvas;
        [Header("Numeronäppäimistö")]
        [SerializeField] GameObject numberInputCanvas;
        [Header("Lisämaksut")]
        [SerializeField] GameObject extraFeeCanvas;
        #endregion
        #region Refs 
        //Price manager go.
        [SerializeField] GameObject priceManager;
        [SerializeField] GameObject statusManager;
        PriceUpdate priceInstance; 
        public PriceUpdate PriceInstance { get { return priceInstance; } }
        PriceStatusManager statusInstance;
        public PriceStatusManager StatusIntance { get { return statusInstance; } }
        #endregion
        public bool EndComission;
        #region Initialize
        //Subscribe to necessary events.
        void Awake()
        {
            GetRefs();
            status = State.Free;
            InitializeEvents();
        }
        void OnDestroy()
        {
            Unsubscribe();
        }
        void GetRefs()
        {
            statusInstance = References.GetRef(gameObject, statusManager, statusInstance);
            priceInstance = References.GetRef(gameObject, priceManager, priceInstance);
        }
        void InitializeEvents()
        {
            Actions.NewTax += ManageNewTax;
            Actions.NavigationButton += ManageClick;
            Actions.Input += ProcessInput;
            Actions.MoveToThis += ForceMoveTo;
            Actions.PaymentSelection += ManagePaymentSelection;
            Actions.ComissionCompleted += CheckComissionCompletion;
        }
        void Unsubscribe()
        {
            Actions.NewTax -= ManageNewTax;
            Actions.NavigationButton -= ManageClick;
            Actions.Input -= ProcessInput;
            Actions.MoveToThis -= ForceMoveTo;
            Actions.PaymentSelection -= ManagePaymentSelection;
            Actions.ComissionCompleted -= CheckComissionCompletion;
        }
        #endregion
        #region Tax selection
        void ManageNewTax(Tax newTax)
        {
            new TaxSelectFunction(this, newTax).Operate();
        }
        #endregion
        #region Button click
        //Do proper function when navigation button is pressed.
        void ManageClick(ButtonType type)
        {
            NavigationExecution executor = new NavigationExecution();
            executor.Apply(type, this);
        }
        void ManagePaymentSelection(Payments.Method type, bool isSecondary)
        {
            NavigationExecution executor = new NavigationExecution();
            executor.Apply(type, isSecondary, this);
        }
        void CheckComissionCompletion()
        {
            if(status != State.Free)
            {
                EndComission = true;
            }
        }
        #endregion Button click
        internal GameObject GetCanvasFromButtonType(ButtonType type)
        {
            return type switch
            {
                ButtonType.Tax => GetTaxCanvasFromStatus(),
                ButtonType.Vapaa => freeStateCanvas,
                ButtonType.Kassa => checkOutGeneralCanvas,
                ButtonType.Options => status == State.Comission ? optionsCanvasOnComission : optionsCanvasCheckOut,
                ButtonType.NumberInput => numberInputCanvas,
                ButtonType.KustPk => numberInputCanvas,
                ButtonType.ExtraFee => extraFeeCanvas,
                ButtonType.Pay => paymentCanvas,
                _ => null
            };
        }
        internal GameObject GetTaxCanvasFromStatus()
        {
            if(status == State.Free)
            {
                return taxCanvasPrelim;
            }
            else
            {
                return taxCanvasOnMove;
            }
        }
        internal GameObject GetPaymentCanvasFromView(PaymentView view)
        {
            return view switch
            {
                PaymentView.Payment => paymentCanvas,
                PaymentView.Secondary => paymentCanvasSecondary,
                PaymentView.Execution => paymentExecutionCanvas,
                _ => paymentCanvas
            };
        }
        internal GameObject GetPaymentCanvasFromView()
        {
            return paymentView switch
            {
                PaymentView.Payment => paymentCanvas,
                PaymentView.Secondary => paymentCanvasSecondary,
                PaymentView.Execution => paymentExecutionCanvas,
                _ => paymentCanvas
            };
        }
        internal GameObject GetPaymentCanvasFromBool(bool isSecondary)
        {
            if(isSecondary)
            {
                return paymentCanvasSecondary;
            }
            else
            {
                return paymentCanvas;
            }
        }
        internal GameObject GetCanvasFromStatus()
        {
            return status switch
            {
                State.Free => freeStateCanvas,
                State.Comission => onComissionCanvas,
                State.Checkout => checkOutGeneralCanvas,
                State.Payment => paymentCanvas, 
                _ => null
            };
        }
        internal GameObject GetCanvasFromStatus(State state)
        {
            return state switch
            {
                State.Free => freeStateCanvas,
                State.Comission => onComissionCanvas,
                State.Checkout => checkOutGeneralCanvas,
                State.Payment => paymentCanvas, 
                _ => null
            };
        }
        void ForceMoveTo(ButtonType type)
        {
            ManageClick(type);
        }
        public void ForceNewStatus(State state)
        {
            this.status = state;
            Actions.ChangeNavigationState(state);
        }
        public void UpdatePaymentView(PaymentView view)
        {
            paymentView = view;
        }
        #region Input
        void ProcessInput(InputMode mode, int num)
        {
            new ExitInput(this, mode).Operate();
        }
        #endregion Input
    }
}
