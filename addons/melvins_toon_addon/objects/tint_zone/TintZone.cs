using Godot;
using System;

namespace MelvinsToonShader
{
    public partial class TintZone : Area3D
    {
        [Export] public int TintPriority;
        [Export] public Color BrightAddColor;
        [Export] public Color BrightMultColor;
        [Export] public Color DarkAddColor;
        [Export] public Color DarkMultColor;
        [Export] public Color SpecularAddColor;
        [Export] public Color SpecularMultColor;
        [Export] public Color RimAddColor;
        [Export] public Color RimMultColor;
        [Export] public Color AnisotropyAddColor;
        [Export] public Color AnisotropyMultColor;
        [Export] public Aabb VisibilityAabb;
        [Export] public VisibleOnScreenNotifier3D VisibilityObject;

        public void ScreenEntered()
        {
            Monitorable = true;
        }
        public void ScreenExited()
        {
            Monitorable = false;
        }
    }
}