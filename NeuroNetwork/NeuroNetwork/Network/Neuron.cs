namespace NeuroNetwork.Network
{

    public class Neuron
    {
        /// <summary>
        /// Все входы нейрона
        /// </summary>
        public Link[] IncomingLinks;

        /// <summary>
        /// Накопленный нейроном заряд 
        /// </summary>
        /// <value>Заряд</value>
        public double Power { get; set; }
    }
}