using System.Text;
using System.Threading;
using Newtonsoft.Json;
using UnityEngine;

namespace BlueSnake.File{
    public class FileJsonWrapper<T> where T : struct {
        
        public T? Entity;
        
        private string _filePath;
        private int _saveDelayMillis;
        private bool _savingDelayed;
        
        public FileJsonWrapper(string filePath, int saveDelayMillis) {
            _filePath = filePath;
            _saveDelayMillis = saveDelayMillis;
            Load();
        }

        public void Load() {
            string path = GetFullPath();
            if (System.IO.File.Exists(path)) {
                Entity = JsonConvert.DeserializeObject<T>(System.IO.File.ReadAllText(path, Encoding.UTF8));
            } else {
                Entity = CreateDefault();
                if (Entity != null) {
                    Save();
                }
            }
        }

        public void Save() {
            System.IO.File.WriteAllText(GetFullPath(), JsonConvert.SerializeObject(Entity, Formatting.Indented));
        }

        public void SaveDelayed() {
            if (_savingDelayed) {
                return;
            }
            _savingDelayed = true;
            ThreadPool.QueueUserWorkItem(_ => {
                Thread.Sleep(_saveDelayMillis);
                Save();
                _savingDelayed = false;
            });
        }

        protected virtual T? CreateDefault() {
            return null;
        }

        public string GetFullPath() {
            return Application.persistentDataPath + "/" + _filePath;
        }
    }
}