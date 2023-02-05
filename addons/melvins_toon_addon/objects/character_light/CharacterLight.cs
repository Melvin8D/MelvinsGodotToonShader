using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MelvinsToonShader
{
    [Tool]
    public partial class CharacterLight : Node3D
    {
        [Export] public ShaderMaterial[] Materials = new ShaderMaterial[] { };
        private IList<TintZone> CurrentTintZones = new List<TintZone>();
        private Color Black = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        private Color White = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        public override void _Process(double delta)
        {
            SetLightDir(Materials);
        }
        public Vector3 Vector3FromAngleYX(float y, float x)
        {
            float negCosX = -Mathf.Cos(x);
            return new Vector3(negCosX * Mathf.Sin(y), Mathf.Sin(x), negCosX * Mathf.Cos(y));
        }
        public void SetLightDir(ShaderMaterial[] materials)
        {
            if (materials.Count() > 0)
            {
                Vector3 characterLightDir = Vector3FromAngleYX(Rotation.Y, Rotation.X);
                foreach (ShaderMaterial material in materials)
                {
                    if (material != null)
                    {
                        material.SetShaderParameter("character_light", characterLightDir);
                    }
                }
            }
        }
        public void SetTint(ShaderMaterial[] materials)
        {
            if (materials.Count() > 0)
            {
                int highestPriority = -1;
                TintZone chosenTintZone = new TintZone();
                if (CurrentTintZones.Count == 0)
                {
                    foreach (ShaderMaterial material in materials)
                    {
                        SetTintParameters(material, Black, Black, Black, Black, Black, White, White, White, White, White);
                    }
                }
                else
                {
                    foreach (TintZone tintZone in CurrentTintZones)
                    {
                        if (tintZone.TintPriority > highestPriority)
                        {
                            highestPriority = tintZone.TintPriority;
                            chosenTintZone = tintZone;
                        }
                    }
                    foreach (ShaderMaterial material in materials)
                    {
                        if (material != null)
                        {
                            Color chosenBrightAdd = chosenTintZone.BrightAddColor;
                            Color chosenDarkAdd = chosenTintZone.DarkAddColor; ;
                            Color chosenSpecularAdd = chosenTintZone.SpecularAddColor; ;
                            Color chosenRimAdd = chosenTintZone.RimAddColor; ;
                            Color chosenAnisotropyAdd = chosenTintZone.AnisotropyAddColor;
                            Color chosenBrightMult = chosenTintZone.BrightMultColor; ;
                            Color chosenDarkMult = chosenTintZone.DarkMultColor; ;
                            Color chosenSpecularMult = chosenTintZone.SpecularMultColor;
                            Color chosenRimMult = chosenTintZone.RimMultColor; ;
                            Color chosenAnisotropyMult = chosenTintZone.AnisotropyMultColor; ;
                            SetTintParameters(material, chosenBrightAdd, chosenDarkAdd, chosenSpecularAdd, chosenRimAdd, chosenAnisotropyAdd, chosenBrightMult, chosenDarkMult, chosenSpecularMult, chosenRimMult, chosenAnisotropyMult);
                        }
                    }
                }
            }
        }
        public void TintzoneEntered(Area3D area)
        {
            if (area is TintZone)
            {
                CurrentTintZones.Add((TintZone)area);
                SetTint(Materials);
            }

        }
        public void TintzoneExited(Area3D area)
        {
            if (area is TintZone)
            {
                CurrentTintZones.Remove((TintZone)area);
                SetTint(Materials);
            }

        }
        public void SetTintParameters(ShaderMaterial material, Color brightAdd, Color brightMult, Color darkAdd, Color darkMult, Color specularAdd, Color specularMult, Color rimAdd, Color rimMult, Color anisotropyAdd, Color anisotropyMult)
        {
            material.SetShaderParameter("bright_scene_add_tint", brightAdd);
            material.SetShaderParameter("dark_scene_add_tint", brightMult);
            material.SetShaderParameter("specular_scene_add_tint", darkAdd);
            material.SetShaderParameter("rim_scene_add_tint", darkMult);
            material.SetShaderParameter("anisotropy_scene_add_tint", specularAdd);
            material.SetShaderParameter("bright_scene_mult_tint", specularMult);
            material.SetShaderParameter("dark_scene_mult_tint", rimAdd);
            material.SetShaderParameter("specular_scene_mult_tint", rimMult);
            material.SetShaderParameter("rim_scene_mult_tint", anisotropyAdd);
            material.SetShaderParameter("anisotropy_scene_mult_tint", anisotropyMult);
        }
    }
}