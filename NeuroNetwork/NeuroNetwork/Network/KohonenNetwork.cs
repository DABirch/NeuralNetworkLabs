using System;
using System.IO;
using System.Drawing;

namespace NeuroNetwork.Network
{
    /// <summary>
    /// ????????? ???? ????????
    /// </summary>
    public class KohonenNetwork
    {
        private readonly Input[] _inputs;
        private readonly Neuron[] _neurons;

        /// <summary>
        /// Initializes a new instance of the <see cref="NeuroNetwork.Network.KohonenNetwork"/> class.
        /// </summary>
        /// <param name="inputCount">??????? ?????????? ????????.</param>
        /// <param name="outputCount">???????? ?????.</param>
        public KohonenNetwork(int inputCount, int outputCount)
        {
            _neurons = new Neuron[outputCount];
            for (var i = 0; i < outputCount; i++)
            {
                _neurons[i] = new Neuron {IncomingLinks = new Link[inputCount]};
            }

            _inputs = new Input[inputCount];
            for (var i = 0; i < inputCount; i++)
            {
                var inputNeuron = new Input();

                inputNeuron.OutgoingLinks = new Link[outputCount];
                for (var j = 0; j < outputCount; j++)
                {
                    var link = new Link
                                   {
                                       Neuron = _neurons[j]
                                   };
                    inputNeuron.OutgoingLinks[j] = link;
                    _neurons[j].IncomingLinks[i] = link;
                }

                _inputs[i] = inputNeuron;
            }
        }

        /// <summary>
        /// ?????????? ?????? ????? ????????? ????. 
        /// </summary>
        /// <param name="input">?????? ???????? ????????.</param>
        public int Handle(int[] input)
        {
            for (var i = 0; i < _inputs.Length; i++)
            {
                var inputNeuron = _inputs[i];
                foreach (var outgoingLink in inputNeuron.OutgoingLinks)
                {
                    outgoingLink.Neuron.Power += outgoingLink.Weight * input[i];
                }
            }

            var maxIndex = 0;
            for (var i = 1; i < _neurons.Length; i++)
            {
                if (_neurons[i].Power > _neurons[maxIndex].Power)
                    maxIndex = i;
            }

            foreach (var outputNeuron in _neurons)
            {
                outputNeuron.Power = 0;
            }

            return maxIndex;
        }

        /// <summary>
        /// ??????? ????????? ????
        /// </summary>
        /// <param name="input">??????? ?????? ???????? ????????.</param>
        /// <param name="correctAnswer">?????????? ?????.</param>
        public void Study(int[] input, int correctAnswer)
        {
            var neuron = _neurons[correctAnswer];
            for (var i = 0; i < neuron.IncomingLinks.Length; i++)
            {
                var incomingLink = neuron.IncomingLinks[i];
                incomingLink.Weight = incomingLink.Weight + 0.5 * (input[i] - incomingLink.Weight);
            }
        }

        /// <summary>
        /// ????????? ????????? ????????
        /// </summary>
        /// <param name="folderPath">???? ?????????</param>
        public void Save(string folderPath)
        {
            var size = (int) Math.Sqrt(_inputs.Length);
            for (var neuronIndex = 0; neuronIndex < _neurons.Length; neuronIndex++)
            {
                var outputNeuron = _neurons[neuronIndex];
                var bitmap = new Bitmap(size, size);
                for (var i = 0; i < outputNeuron.IncomingLinks.Length; i++)
                {
                    var incomingLink = outputNeuron.IncomingLinks[i];
                    var eightBitColor = (byte) incomingLink.Weight;
                    var color = Color.FromArgb(255, eightBitColor, eightBitColor, eightBitColor);
                    var y = i/size;
                    var x = i%size;
                    bitmap.SetPixel(x, y, color);
                }
                bitmap.Save(Path.Combine(folderPath, neuronIndex.ToString() + ".bmp"));
            }
        }
    }
}