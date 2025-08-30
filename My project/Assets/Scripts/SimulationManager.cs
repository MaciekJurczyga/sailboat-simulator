using UnityEngine;

public class SimulationManager : MonoBehaviour
{
 
    public BoatController boatController; 
    public GraphDrawer graphDrawer;
    public WindIndicatorController windIndicatorController;

    private WindSystem _windSystem;
    private PhysicsModel _physicsModel;
    private GraphPointsWrapper _graphPointsWrapper;
    private PhysicsCalculator _physicsCalculator;

    void Awake() 
    {

        _windSystem = new WindSystem();
        _physicsCalculator = new PhysicsCalculator();
        
        _physicsModel = new PhysicsModel(_windSystem, _physicsCalculator);
        _physicsModel.LoadModel(); 
        
        _graphPointsWrapper = new GraphPointsWrapper();
        _graphPointsWrapper.loadPoints(_physicsModel.GetBoatDataForBestLD());
        
    }

    void Start()
    {
        
        if (boatController != null)
        {
            boatController.Initialize(_physicsModel, _windSystem, _graphPointsWrapper);
        }
        else
        {
            Debug.LogError("Boat controller is not assigned to SimulationManager!", this);
        }

        if (graphDrawer != null)
        {
           
            graphDrawer.Initialize(_graphPointsWrapper);
            graphDrawer.DrawGraph();
        }

        if (windIndicatorController != null)
        {
            windIndicatorController.Initialize(_windSystem);
        }
        else
        {
            Debug.LogError("GrapDrawer controller is not assigned to SimulationManager!", this);
        }
    }
    
    public WindSystem GetWindSystem() => _windSystem;
    public PhysicsModel GetPhysicsModel() => _physicsModel;
    public GraphPointsWrapper GetGraphPointsWrapper() => _graphPointsWrapper;
}