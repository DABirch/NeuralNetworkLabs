namespace NeuroNetwork.Network
{ 
    /// <summary>
    /// Связь входа с нейроном
    /// </summary>
    public class Link
    {
        /// <summary>
        /// Нейрон
        /// </summary>
        public Neuron Neuron;

        /// <summary>
        /// Вес связи
        /// </summary>
        public double Weight;
    }
}