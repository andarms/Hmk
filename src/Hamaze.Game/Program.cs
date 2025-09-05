using Hamaze.Engine.Core;
using Hamaze.Game.Pong;
using Hamaze.Game.Scenes;

Settings.SetTitle("Hamaze Game");
Game.Initialize();
SceneManager.AddScene(new TestRoomScene());
SceneManager.SwitchTo<TestRoomScene>();
Game.Run();