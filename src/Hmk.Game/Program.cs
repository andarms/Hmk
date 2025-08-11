using Hmk.Engine.Core;
using Hmk.Engine.Scenes;
using Hmk.Game.Scenes;


SceneManager.AddScene(new GameplayScene());
SceneManager.SwitchTo<GameplayScene>();
Game.Run();