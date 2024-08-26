using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameEditor;
using Sirenix.OdinInspector.Editor;

namespace GameDevKitEditor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Networking;
    [TreeWindow("GPT工具")]
    public class GPTWindow  : OdinEditorWindow
    {
        private string m_ApiUrl = "https://api.openai.com/v1/chat/completions";

        public string m_OpenAI_Key = "";
        
        public string m_gptModel = "gpt-3.5-turbo";

        [LabelText("对话结果")]
        [TextArea(5, 10)]
        public string chatResult;

        [LabelText("对话文本")]
        [TextArea(5, 10)]
        public string chatText;
        //按钮大,绿色
        [Button(ButtonSizes.Large, ButtonStyle.FoldoutButton)]
        public async void Chat()
        {
            var data = await GetPostData(chatText);
            Debug.Log(data);
            chatResult = data;
        }
        [Serializable]public class PostData
        {
            public string model;
            public List<SendData> messages;
        }
        [Serializable]
        public class SendData
        {
            public string role;
            public string content;
            public SendData() { }
            public SendData(string _role,string _content) {
                role = _role;
                content = _content;
            }
 
        }
        //gpt返回的数据类
        [System.Serializable]
        public class ChatCompletion
        {
            public string id;
            public string @object;
            public int created;
            public string model;
            public Usage usage;
            public Choice[] choices;

            [System.Serializable]
            public class Usage
            {
                public int prompt_tokens;
                public int completion_tokens;
                public int total_tokens;
            }

            [System.Serializable]
            public class Choice
            {
                public Message message;
                public string finish_reason;
                public int index;

                [System.Serializable]
                public class Message
                {
                    public string role;
                    public string content;
                }
            }
        }

     
        public async Task<string> GetPostData(string _postWord)
        {
            //缓存发送的信息列表
    
            using (UnityWebRequest request = new UnityWebRequest(m_ApiUrl, "POST"))
            {
                PostData postData = new PostData
                {
                    model = m_gptModel,
                    messages = new List<SendData>(){new SendData("user",_postWord)}
                };

                string _jsonText = JsonUtility.ToJson(postData);
                byte[] data = System.Text.Encoding.UTF8.GetBytes(_jsonText);
                request.uploadHandler =new UploadHandlerRaw(data);
                request.downloadHandler =new DownloadHandlerBuffer();
 
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", string.Format("Bearer {0}", m_OpenAI_Key));
                var task = request.SendWebRequest().ToUniTask();
                await task;
                if (request.responseCode == 200)
                {
                    string _msg = request.downloadHandler.text;
                    ChatCompletion chatCompletion = JsonUtility.FromJson<ChatCompletion>(_msg);
                    string content = chatCompletion.choices[0].message.content;
                    //解析chatgpt返回的数据,
                 
                    return content;
                }
                else
                {
                    Debug.LogError(request.error);
                }
                return "";
            }
        }
    }
}