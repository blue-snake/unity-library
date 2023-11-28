using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace BlueSnake.File{
    public class FileJsonWrapper<T> where T : struct {
        
        public T? Entity;
        
        private string _filePath;
        private int _saveDelayMillis;
        private bool _savingDelayed;
        
        public FileJsonWrapper(string filePath, int saveDelayMillis) {
            _filePath = Application.persistentDataPath + "/" + filePath;;
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

        public async void SaveDelayed() {
            if (_savingDelayed) {
                return;
            }
            _savingDelayed = true;
            await Task.Run(() => {
                Task.Delay(_saveDelayMillis).Wait();
                Save();
                _savingDelayed = false;
            });
        }

        protected virtual T? CreateDefault() {
            return null;
        }

        public string GetFullPath() {
            return _filePath;
        }
    }
}