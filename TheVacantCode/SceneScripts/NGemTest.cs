/*using BaseLib.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

namespace TheVacant.TheVacantCode.SceneScripts;

public partial class NGemTest : Control
{
	// TODO:
	// Change the SCENE_PATH to match where you put the gem_test.tscn
	// Open gem_test.tscn and fix the path to the image at the top.
	// Also fix the namespace at the top of this file.

	private const string SCENE_PATH = "res://TheVacant/scenes/gems/gem_test.tscn";
	private Control _gemVisuals = null!;
	private float _rotationSpeed = 10f;
	
	// These are static Create functions to set up a working scene. You will find this in many of MegaCrits "N" classes, like NOrb.
	public static NGemTest Create()
	{
		var nGem = PreloadManager.Cache.GetScene(SCENE_PATH).Instantiate<NGemTest>();
		nGem._rotationSpeed = 30f;
		return nGem;
	}
	/// negative floats will make it spin counter-clockwise
	public static NGemTest Create(float rotationSpeed)
	{
		var nGem = PreloadManager.Cache.GetScene(SCENE_PATH).Instantiate<NGemTest>();
		nGem._rotationSpeed = rotationSpeed;
		return nGem;
	}

	// connect private fields to the nodes in the scene. The "%" allows for direct access by name.
	// This has to be enabled on nodes in godot (right click -> "Access as Unique Name)
	public override void _Ready()
	{
		_gemVisuals = GetNode<Control>("%GemVisuals");
	}

	// happens every frame. Rotate.
	public override void _Process(double delta)
	{
		_gemVisuals.RotationDegrees += (float)delta * _rotationSpeed;
	}
	
	// Just a quick AddedNode to display this in the main menu. You most likely won't be doing it this way.
	private static readonly AddedNode<NMainMenu, NGemTest> Gems = new(mainMenu =>
	{
		var nGem = Create(-50f);
		mainMenu.AddChildSafely(nGem);
		nGem.Position = new  Vector2(500.0f, 500.0f);
		return nGem;
	});
}
*/