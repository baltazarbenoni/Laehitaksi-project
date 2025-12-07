using UnityEngine;
//C 2025 Daniel Snapir alias Baltazar Benoni

namespace Navigation
{
    public interface INavigation
    {
        public void Operate();
    }
    //Parent class for different navigation processes.
    public class NavigationFunction : INavigation
    {
        public NavigationFunction(NavigationManager manager)
        {
            this.manager = manager;
            this.function = ActivateDeactivate;
        }
        //Constructor to create empty.
        public NavigationFunction()
        {
            this.function = Empty;
        }
        protected delegate void MyDelegate();
        protected MyDelegate function;
        protected NavigationManager manager;
        protected GameObject activate;
        protected GameObject deactivate;
        public void Operate()
        {
            this.function();
        }
        public void ActivateDeactivate()
        {
            if(activate == null || deactivate == null)
            {
                return;
            }
            activate.SetActive(true);
            deactivate.SetActive(false);
        }
        public void ChangeActivation()
        {
            if(activate == null)
            {
                return;
            }
            activate.SetActive(!activate.activeSelf);
        }
        public void ChangeActivationBoth()
        {
            if(activate == null || deactivate == null)
            {
                return;
            }
            activate.SetActive(!activate.activeSelf);
            deactivate.SetActive(!activate.activeSelf);
        }
        public void ForceStatus(State state)
        {
            manager.ForceNewStatus(state);
        }
        public void Empty()
        {
            return;
        }
    }

}
