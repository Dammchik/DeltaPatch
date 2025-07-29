using System;
using System.Collections.Generic;

namespace DeltaPatchCore.Delta
{
    public class History
    {
        private readonly Stack<DeltaPatch> _undoStack = new();
        private readonly Stack<DeltaPatch> _redoStack = new();

        public void Record(byte[] oldState, byte[] newState)
        {
            var patch = DeltaPatch.Create(oldState, newState);
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
}
