using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace BeatTogether.Patches
{
    [HarmonyPatch(typeof(PlayerHeightDetector), "LateUpdate")]
	public static class LateUpdatePatch
	{
		private static FieldInfo _headPosToPlayerHeightOffset = typeof(PlayerHeightDetector.InitData)
			.GetField("headPosToPlayerHeightOffset", BindingFlags.Instance | BindingFlags.Public);
		private static FieldInfo _computedPlayerHeight = typeof(PlayerHeightDetector)
			.GetField("_computedPlayerHeight", BindingFlags.Instance | BindingFlags.NonPublic);

		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var instructionList = instructions.ToList();
			for (var i = 0; i < instructionList.Count; i++)
			{
				if (instructionList[i].opcode == OpCodes.Add &&
					instructionList[i - 1].LoadsField(_headPosToPlayerHeightOffset))
				{
					instructionList.RemoveRange(i - 3, 4);
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
