using KD.CQRS.Core;
using System;

namespace KD.CQRS.TestApp
{
    public class SampleQueryModel
    {
        public int Value { get; set; }
    }

    public interface ISampleQuery : IQuery<SampleQueryModel, bool>
    {
    }

    public interface ISampleCommand : ICommand<SampleQueryModel>
    {
    }

    public class SampleQuery : ISampleQuery
    {
        public bool Execute(SampleQueryModel input)
        {
            return input.Value % 2 == 0;
        }
    }

    public class SampleCommand : ISampleCommand
    {
        public object Execute(SampleQueryModel source)
        {
            Console.WriteLine(source.Value);
            return null;
        }
    }
}
