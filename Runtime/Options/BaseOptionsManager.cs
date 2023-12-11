using System;
using System.IO;
using BlueSnake.File;
using UnityEngine;

namespace BlueSnake.Options {
    public abstract class BaseOptionsManager<T> : MonoBehaviour {
        [Header("Properties")]
        [SerializeField]
        private string path = "options.json";

        [SerializeField]
        private int saveDelayMilliseconds = 5000;

        protected OptionsFile<T> file;

        public virtual void Awake() {
            file = new OptionsFile<T>(path, saveDelayMilliseconds, CreateDefault);
        }

        public void Save() {
            file.Save();
        }

        public void SaveDelayed() {
            file.SaveDelayed();
        }

        public T GetOptions() {
            return file.Entity;
        }
        
        protected abstract T CreateDefault();
    
    }

    public class OptionsFile<T> : FileJsonWrapper<T> {
        private readonly OptionsFileCreateCallback _callback;

        public OptionsFile(string filePath, int saveDelayMillis, OptionsFileCreateCallback callback) : base(filePath,
            saveDelayMillis) {
            _callback = callback;
            Load();
        }

        protected override T CreateDefault() {
            return _callback.Invoke();
        }


        public delegate T OptionsFileCreateCallback();
    }
}