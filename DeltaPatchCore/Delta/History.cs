using DeltaPatchCore.Delta;
using DeltaPatchCore.Interfaces;

public class History
{
    private readonly Stack<DeltaPatch> _undoStack = new();
    private readonly Stack<DeltaPatch> _redoStack = new();
    private readonly IDiffStrategy _strategy;

    public History(IDiffStrategy strategy)
    {
        _strategy = strategy;
    }

    public void Record(byte[] oldData, byte[] newData)
    {
        var patch = _strategy.ComputeDiff(oldData, newData);
        _undoStack.Push(patch);
        _redoStack.Clear();
    }

    public bool CanUndo => _undoStack.Count > 0;
    public bool CanRedo => _redoStack.Count > 0;

    public void Undo(byte[] data)
    {
        if (!CanUndo) return;

        var patch = _undoStack.Pop();
        patch.Apply(data);
        _redoStack.Push(patch);
    }

    public void Redo(byte[] data)
    {
        if (!CanRedo) return;

        var patch = _redoStack.Pop();
        patch.Apply(data);
        _undoStack.Push(patch);
    }
}
