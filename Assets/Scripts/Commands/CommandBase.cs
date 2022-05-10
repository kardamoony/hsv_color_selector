namespace Commands
{
    public abstract class CommandBase
    {
        protected CommandBase(){}

        public abstract void Execute();
    }

    public abstract class UndoableCommand : CommandBase
    {
        public abstract void Undo();
    }
}
