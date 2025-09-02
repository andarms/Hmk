using Hamaze.Engine.Core;
using Hamaze.Game.Pong;
using Hamaze.Game.Scenes;

Settings.SetTitle("Hamaze Game");
Game.Initialize();
SceneManager.AddScene(new PongGameScene());
SceneManager.SwitchTo<PongGameScene>();
Game.Run();