namespace Examples.DLR.Expressions
{
    internal class Computer
    {
        public Computer(string cpu, int memorySizeGbtye)
        {
            this.Cpu = cpu;
            this._memotySizeGByte = memorySizeGbtye;
        }

        public string Cpu { get; private set; }

        private readonly int _memotySizeGByte;

        public override string ToString()
        {
            return $"Computer {{ Cpu = {Cpu}, Memory = {_memotySizeGByte} GB }}";
        }
    }

}
