using System;
using Navigation;
using Taxes;
using Input;
using UnityEngine;

public static class Actions
{
    #region Tax change
    public static Action<TaxButton> TaxSelect;
    public static Action<Tax> NewTax;
    public static Action<Tax> BackToBase;
    #endregion
    #region Navigation
    public static Action<ButtonType> NavigationButton;
    public static Action<ButtonType> MoveToThis;
    public static Action<State> ChangeNavigationState;
    #endregion
    #region Input
    public static Action<int> NumberInput;
    public static Action<int> SetNumberInputMode;
    public static Action<InputMode, int> Input;
    #endregion
    #region Payment
    public static Action<Payments.Method, bool> PaymentSelection;
    public static Action<InputMode, int> PaymentInput;
    public static Action<InputType> SecondaryPayment;
    public static Action<string> CreateReceipt;
    public static Action PaymentAnimationFinished;
    #endregion
    #region Price status/changes
    public static Action<float> UpdatePrice;
    public static Action<float> ForcePrice;
    //Action to update 'PriceUpdate' class from the 'PriceStatusManager' class. Needed especially in the case of extra fee addition/removal.
    public static Action<float> AddExtraFeeOrPayment;
    public static Action<float> PriceUpdateForceSum;
    //'PriceStatus' class is reinstantiated at the end of comission. This notifies other classes.
    public static Action<PriceStatus> NewPriceStatus;
    //Event to trigger the ending of comission mode.
    public static Action ComissionCompleted;
    #endregion
    #region Extras
    //Events triggered by extra fee addition and removal.
    public static Action<ExtraFee> AddFee;
    public static Action RemoveExtras;
    #endregion
    #region Misc
    public static Action<float> SpeedChange;
    #endregion
}
