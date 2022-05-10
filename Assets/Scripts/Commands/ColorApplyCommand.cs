using HSVColorSelector;
using UnityEngine;

namespace Commands
{
    public class ColorApplyCommand : UndoableCommand
    {
        private readonly ColorTargetBase _colorTarget;
        private readonly Color _previousColor;
        
        public Color Color { get;}
        
        public ColorApplyCommand(Color color, Color previousColor, ColorTargetBase colorTarget)
        {
            Color = color;
            _previousColor = previousColor;
            _colorTarget = colorTarget;
        }

        public override void Execute()
        {
            _colorTarget.ApplyColor(Color);
        }

        public override void Undo()
        {
            _colorTarget.ApplyColor(_previousColor);
        }
    }
}
