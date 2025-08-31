namespace Hamaze.Engine.Core;

public class Scene
{
  protected readonly World world = new();

  public virtual Color BackgroundColor { get; } = Color.Black;



  public bool IsActive { get; protected set; } = true;
  public bool IsPaused { get; protected set; } = false;


  public virtual void Initialize()
  {
    world.Initialize();
  }

  public virtual void OnEnter()
  {
    IsActive = true;
    IsPaused = false;
  }

  public virtual void OnExit()
  {
    IsActive = false;
  }

  public virtual void OnPause()
  {
    IsPaused = true;
  }

  public virtual void OnResume()
  {
    IsPaused = false;
  }

  public virtual void Update(float dt)
  {
    world.Update(dt);
  }

  public virtual void Draw()
  {
    world.Draw();
  }

  public virtual void Dispose()
  {
    world.Terminate();
  }
}