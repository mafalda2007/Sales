namespace Sales.Infrastructure
{
    using ViewModels;

    /* 
     * El unico objetivo es Instanciar una sola instancia de la MainViewModel 
     */
    public class InstanceLocator
    {
        public MainViewModel Main { get; set; }

        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        }
    }
}
