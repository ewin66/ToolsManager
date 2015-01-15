using Atlante.Common;
using Atlante.Mef.Interfaces;
using System.Linq;
using ToolsManager.DataModel.Common;

namespace ToolsManager.Client
{
	public static class ParameterExtensions
	{
		private const string SHARED_PATH_PARAMETER = "SharedPath";

		public static void ConfigureParameters(this ITool taskEngine, TaskInfo task)
		{
			taskEngine.RemoveDeprecatedParameters(task);

			taskEngine.UpdateParametersInfo(task);

			if (!taskEngine.CanShareFiles)
				return;

			var sharedPathParameter = task.Parameters.Items.Where(p => p.Key == "SharedPath").FirstOrDefault();

			if (sharedPathParameter == null)
				task.Parameters.Add(new Parameter() { Key = "SharedPath", IsSystem = true, ParameterType = ParameterType.FilePath });
		}

		public static void UpdateParametersInfo(this ITool taskEngine, TaskInfo task)
		{
			foreach (var param in taskEngine.GetParametersDefinition())
			{
				var addedParameter = task.Parameters.Items.Where(p => p.Key == param.Key).FirstOrDefault();

				if (addedParameter != null)
				{
					//Ensure parameter configuration values                    
					addedParameter.Options = param.Options;
					addedParameter.ParameterType = param.ParameterType;

					continue;
				}

				var newParameter = new Parameter() { Key = param.Key, Options = param.Options, ParameterType = param.ParameterType, Value = param.DefaultValue };

				//UpdateDefaultValue(newParameter, param.DefaultValue, param.ParameterType);

				task.Parameters.Add(newParameter);
			}
		}

		public static void RemoveDeprecatedParameters(this ITool taskEngine, TaskInfo task)
		{
			var validParameters = taskEngine.GetParametersDefinition();

			foreach (var parameter in task.Parameters.Items.Reverse<Parameter>())
			{
				if (parameter.IsSystem)
					continue;

				bool isDeprecated = (validParameters.Where(p => p.Key == parameter.Key).FirstOrDefault() == null);

				if (isDeprecated)
					task.Parameters.Remove(parameter);
			}
		}

		public static string GetSharedPath(this TaskInfo task)
		{
			var parameter = task.Parameters[SHARED_PATH_PARAMETER];

			if (parameter != null)
				return parameter.Value;

			return string.Empty;
		}

		public static void ReplaceVariables(this TaskInfo task, ToolsVariables variables)
		{
			foreach (var parameter in task.Parameters.Items)
			{
				foreach (var variable in variables.Items)
				{
					if (parameter.Value.Contains(variable.Key))
					{
						parameter.Value = parameter.Value.Replace(variable.Key, variable.Value);
					}
				}
			}
		}

		//private static void UpdateDefaultValue(Parameter parameter, string defaultValue, ParameterType type)
		//{
		//    parameter.
		//}
	}
}
