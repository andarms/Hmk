using Hmk.Engine.Core;
using Hmk.Engine.Scenes;
using Hmk.Editor.Scenes;

Game.Initialize();
SceneManager.AddScene(new EditorScene());
SceneManager.SwitchTo<EditorScene>();
Game.Run();
