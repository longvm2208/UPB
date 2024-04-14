using UnityEditor;

namespace ScriptableStateMachine.EditorTools
{
    public class ScriptCreator
    {
        private const string PATH = "Assets/Utils/Scripts/State Machine/Scriptable";

        private const string ACTION_TEMPLATE = "/Editor/Templates/ActionTemplate.cs.txt";
        private const string DECISION_TEMPLATE = "/Editor/Templates/DecisionTemplate.cs.txt";

        [MenuItem("Assets/Create/State Machine/C# Script/Action")]
        public static void CreateNewAcionScript()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(PATH + ACTION_TEMPLATE, "NewAction.cs");
        }

        [MenuItem("Assets/Create/State Machine/C# Script/Action", true)]
        public static bool CreateNewAcionScriptValidate()
        {
            return System.IO.File.Exists(PATH + ACTION_TEMPLATE);
        }

        [MenuItem("Assets/Create/State Machine/C# Script/Decision")]
        public static void CreateNewDecisionScript()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(PATH + DECISION_TEMPLATE, "NewDecision.cs");
        }

        [MenuItem("Assets/Create/State Machine/C# Script/Decision", true)]
        public static bool CreateNewDecisionScriptValidate()
        {
            return System.IO.File.Exists(PATH + DECISION_TEMPLATE);
        }
    }
}
