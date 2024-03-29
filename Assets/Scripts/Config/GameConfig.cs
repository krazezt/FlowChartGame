using System.Numerics;

public class GameConfig {

    public enum AppType {
        EDITOR,
        GAME,
        DEMO
    }

    public enum VariableType {
        Single,
        Array
    }

    public enum GameplayState {
        Playing,
        Simulating
    }

    public enum FunctionBlockType {
        Start,
        Assign,
        ConditionEqual,
        ConditionLess,
        ConditionLessEqual,
        ConditionGreater,
        ConditionGreaterEqual,
        Action,
        Add,
        Subtract,
        Multiply,
        Divide,
        EndBlock,
        Increase,
        Decrease,
        SwitchCase,
    }

    public enum TestCaseType {
        Normal,
        Special
    }

    public const int MAX_STEP = 50;
    public const float DRAG_LATENCY = 0.3f;
    public const float MAX_HOLD_OFFSET = 0.5f;
    public const float VISUALIZE_SEGMENT_DURATION = 0.5f;
    public const float VISUALIZE_GAP_SEGMENT_DURATION = 1f;
    public const float VISUALIZE_GAP_RESULT_DURATION = 3f;

    public const float POPUP_START_SCALE = 0.7f;
    public const float POPUP_DURATION = 0.3f;
    public const float BACKDROP_FADE_ALPHA = 0.9f;
    public const float VARIABLE_PANEL_SEGMENT_HEIGHT = 180f;
    public const float VARIABLE_PANEL_WIDTH = 170f;
    public static readonly Vector2 VARIABLE_SIZE = new(150, 165);
}