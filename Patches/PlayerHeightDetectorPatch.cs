using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RevertPlayerHeightDetection;

namespace BeatTogether.Patches
{
    [HarmonyPatch(typeof(PlayerHeightDetector), "LateUpdate")]
	public static class LateUpdatePatch
	{
		private static FieldInfo _computedPlayerHeight = typeof(PlayerHeightDetector)
	        .GetField("_computedPlayerHeight", BindingFlags.Instance | BindingFlags.NonPublic);

		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			if (!Plugin.Configuration.Enabled)
				return instructions;

			var instructionList = instructions.ToList();
			for (var i = 0; i < instructionList.Count; i++)
			{
				if (instructionList[i].opcode == OpCodes.Ldc_R4 &&
					instructionList[i + 1].opcode == OpCodes.Add)
				{
					instructionList.RemoveRange(i, 2);
					break;
				}
			}
			for (var i = 0; i < instructionList.Count; i++)
			{
				if (instructionList[i].opcode == OpCodes.Sub &&
					instructionList[i - 1].LoadsField(_computedPlayerHeight))
				{
					instructionList.InsertRange(i, new[]
					{
						new CodeInstruction(OpCodes.Ldc_R4, 1.2f),
						new CodeInstruction(OpCodes.Mul)
					});
					break;
				}
			}
			for (var i = 0; i < instructionList.Count; i++)
			{
				if (instructionList[i].opcode == OpCodes.Callvirt &&
					instructionList[i - 1].LoadsField(_computedPlayerHeight))
				{
					instructionList.InsertRange(i, new[]
					{
						new CodeInstruction(OpCodes.Ldc_R4, 1.2f),
						new CodeInstruction(OpCodes.Mul)
					});
					break;
				}
			}

			return instructionList;
		}
	}
}
