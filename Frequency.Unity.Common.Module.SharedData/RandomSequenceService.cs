using System.Linq;
using Frequency.Framework;

namespace Frequency.Unity.Common.Module.SharedData
{
    public interface IRandomSequenceService
    {
        string Chars { get; }
        string Numbers { get; }
        string GetSequence(string sequence, int length);
    }

    public class RandomSequenceService : IRandomSequenceService
    {
        private readonly IModuleContext context;

        public RandomSequenceService(IModuleContext context)
        {
            this.context = context;
        }

        public string Chars
        {
            get { return "ABCDEFGHIJKLMNOPQRSTUVWXY"; }
        }

        public string Numbers
        {
            get { return "0123456789"; }
        }

        public string GetSequence(string sequence, int length)
        {
            return new string(
                Enumerable.Repeat(sequence, length)
                    .Select(s => s[context.System.Random(0, s.Length)])
                    .ToArray());
        }
    }
}