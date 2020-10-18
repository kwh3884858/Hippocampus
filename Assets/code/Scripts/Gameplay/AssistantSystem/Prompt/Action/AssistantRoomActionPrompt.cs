using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assistant {

    /// <summary>
    /// 助手系统 房间场景 的行为提示
    /// </summary>
    public class AssistantRoomActionPrompt : IAssistantPrompt
    {
        public void DoPrmpt()
        {
            // TODO： 1、判断当前房间可探索内容是否都已经探索过了，如果是，则要求玩家离开当前房间（这样玩家就会进入过道）。
            // 2、判断玩家的证物内，关于尸体的证物都搜集完毕了没。如果还没，则提醒玩家调查尸体。如果搜集完毕，进入下一个判断。
            // 3、判断玩家的证物中，关于场景的证物都搜集完毕了没，如果没，则提醒玩家现在应当搜查场景内的物品。如果搜集完毕，则进入下一个判断。
            // 4、判断玩家是否触发了所有在场NPC的重要对话，如果没，则提醒玩家与在场NPC对话，如果所有对话全部触发完毕，则进入下一个判断。
            // 5、判断这个房间内有没有设置在所有内容全部收集完毕之后的重要剧情。如果有，则提示玩家此项重要剧情。如果没有，则提示玩家此处已经没有什么好搜查的了
        }
    }
}
