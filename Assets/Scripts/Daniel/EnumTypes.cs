using UnityEngine;
using Navigation;
//C 2025 Daniel Snapir alias Baltazar Benoni
    public class EnumNames
    {
        //Get the name of the button.
        public static string GetText(ButtonType type)
        {
            return type switch
            {

                ButtonType.Back => "Takaisin",
                ButtonType.Next => "Seuraava",
                ButtonType.Tax => "Taksa",
                ButtonType.Options => "Valinnat",
                ButtonType.Hold => "Hold",
                ButtonType.AddCustomer => "As. kyytiin",
                ButtonType.Kassa => "Kassa",
                ButtonType.MaxPrice => "Max hinta",
                ButtonType.MaxTime => "Max aika",
                ButtonType.KustPk => "Kust.pk",
                ButtonType.ExtraFee => "Lisät",
                ButtonType.Pay => "Maksu",
                _ => "Unable to get text"
            };
        }
        public static string PaymentName(Payments.Method type)
        {
            return type switch
            {
                Payments.Method.Suorakorvaus => "Suorakorvaus",
                Payments.Method.Sote => "Sotekortti",
                Payments.Method.Partial => "Osittaism.",
                Payments.Method.Divided => "Jaettu m.",
                Payments.Method.Taxcard => "Taksikortti",
                Payments.Method.Billing => "Laskutus",
                Payments.Method.DebitCredit => "Debit/credit",
                Payments.Method.BypassCash => "Ohita (kät)",
                Payments.Method.Cash => "Käteinen",
               _ => "Unable to get text"
            };
 
        }
    }

namespace Navigation
{
    public enum ButtonType
    {
        None,
        Back,
        Next,
        Options,
        Tax,
        TaxOnComission,
        Hold,
        AddCustomer,
        Vapaa,
        Kassa,
        MaxPrice,
        MaxTime,
        KustPk,
        ExtraFee,
        NumberInput,
        Pay  
    }

    public enum AddButton
    {
        None,
        Ennakko1,
        Ennakko2,
        Inva,
        Porras,
        Asema,
        Erikois,
        Muu,
        BusinessClassic
    }
    public enum InputType
    {
        None,
        Addition,
        Customer,
        MaxPrice,
        MaxTime,
        KustPk,
        Partial,
        Divided
    }
    public enum State
    {
        None,
        Free,
        Comission,
        Checkout,
        Payment 
    }
    public enum PaymentView
    {
        None,
        Payment,
        Secondary,
        Execution
    }
}
namespace Payments
{
    public enum Method
    {
        None,
        Suorakorvaus,
        Sote,
        Partial,
        Divided,
        Taxcard,
        Billing,
        DebitCredit,
        BypassCash,
        Cash
    }
    public enum CheckoutOptionsButton
    {
        None,
        Sisavalo,
        Alennus,
        Tippi,
        Hukka,
        KmNollaus,
        ALV,
        Sophinta
    }
}
namespace Instantiation 
{
    public enum TaxPage
    {
        One,
        Two,
        Three,
        OnComission
    }
}
namespace Input
{
    public enum Format
    {
        NORMAL,
        EURO,
        MIN,
        PERCENT
    }
}
namespace Taxes
{
    public enum TaxMode
    {
        Normal,
        Wait,
        Fixed
    }
}