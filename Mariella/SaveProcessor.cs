using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mariella {
    public class SaveProcessor {
        private static readonly byte[] encryption_key = new byte[] { 115, 97, 82, 112, 109, 90, 54, 109, 77, 103, 90, 112, 109, 99, 111, 106, 85, 107, 118, 107, 121, 71, 69, 81, 71, 101, 122, 57, 89, 107, 87, 111, 88, 90, 102, 74, 100, 82, 99, 57, 90, 109, 109, 74, 114, 67, 122, 102, 77, 56, 74, 107, 115, 86, 120, 81, 102, 81, 75, 56, 117, 66, 66, 115 };

        public static string ProcessEncryption(string input) {
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0;index < input.Length;++index) stringBuilder.Append((char)(input[index] ^ encryption_key[index % 0x40]));
            return stringBuilder.ToString();
        }

        public static TSPSave ProcessSaveFile(string path) {
            string encryptedData = System.IO.File.ReadAllText(path);
            string rawsavedata = ProcessEncryption(encryptedData);
            return TSPSave.Deserialize(rawsavedata, path);
        }

        public static TSPSave CreateNew() {
            return new TSPSave();
        }

        public static bool Save(string path, TSPSave save) {
            try {
                if(save.fieldData.StringData.Count > 0) save.fieldData.StringData[0].Value = JsonConvert.SerializeObject(save.saveData, Formatting.Indented);
                string jout = JsonConvert.SerializeObject(save.fieldData, Formatting.None);
                string ecry = ProcessEncryption(jout);
                System.IO.File.WriteAllText(path, ecry);
                return true;
            } catch (Exception e) {
                throw new Exception("Stanley! You've broken my application!\n" + e.Message);
            }
            return false;
        }

    }


    public class TSPKVData {
        public List<KVStructS> StringData { get; set; }
        public List<KVStructI> IntData { get; set; }
        public List<KVStructF> FloatData { get; set; }
        public List<KVStructB> BoolData { get; set; }

        public TSPKVData() {
            this.StringData = new List<KVStructS>();
            this.IntData = new List<KVStructI>();
            this.FloatData = new List<KVStructF>();
            this.BoolData = new List<KVStructB>();
        }
    }

    public class TSPSaveData {
        public List<SaveDataStruct> saveDataCache { get; set; }

        public TSPSaveData() {
            this.saveDataCache = new List<SaveDataStruct>();
        }
    }
        
    public class TSPSave {
        public string path;
        public TSPKVData fieldData;
        public TSPSaveData saveData;

        public static TSPSave Deserialize(string rawData, string path = "") {
            TSPSave savedata = new TSPSave {
                path = path,
                fieldData = JsonConvert.DeserializeObject<TSPKVData>(rawData)
            };
            savedata.saveData = (savedata.fieldData.StringData.Count > 0) ? JsonConvert.DeserializeObject<TSPSaveData>(savedata.fieldData.StringData[0].ReturnValue()) : new TSPSaveData();
            return savedata;
        }

        public TSPSave() {
            path = "";
            fieldData = new TSPKVData();
            saveData = new TSPSaveData();
        }

        public void GetKeyName() {

        }

        public void AddNewFieldData(Type type, string defVal = "") {
            if (type == typeof(string)) {
                this.fieldData.StringData.Add(new KVStructS(defVal));
            } else if (type == typeof(int)) {
                this.fieldData.IntData.Add(new KVStructI(defVal));
            } else if (type == typeof(bool)) {
                this.fieldData.BoolData.Add(new KVStructB(defVal));
            } else if (type == typeof(float)) {
                this.fieldData.FloatData.Add(new KVStructF(defVal));
            } else {
                
            }
        }

        public void RemoveFieldData(Type type, int index) {
            if (type == typeof(string)) {
                this.fieldData.StringData.RemoveAt(index);
            } else if (type == typeof(int)) {
                this.fieldData.IntData.RemoveAt(index);
            } else if (type == typeof(bool)) {
                this.fieldData.BoolData.RemoveAt(index);
            } else if (type == typeof(float)) {
                this.fieldData.FloatData.RemoveAt(index);
            } else {

            }
        }

        public void AddNewSaveData() {
            this.saveData.saveDataCache.Add(new SaveDataStruct());
        }
    }

    public class SaveDataStruct : IDataStruct {
        public string key { get; set; }
        public int configureableType { get; set; }
        public int IntValue { get; set; }
        public float FloatValue { get; set; }
        public bool BooleanValue { get; set; }
        public string StringValue { get; set; }

        public string GetKeyName() {
            return this.key;
        }

        public SaveDataStruct(string defValue = "") {
            this.key = defValue;
            this.StringValue = String.Empty;
        }
    }
    public class KVStruct : IDataStruct {
        public string Key { get; set; }
        public object Value { get; set; }

        [JsonIgnore]
        public Type type = typeof(string);

        public KVStruct(string defVal = "") {
            this.Key = defVal;
            this.Value = 0;
        }

        public string GetKeyName() {
            return this.Key;
        }

        public dynamic ReturnValue() {
            return Convert.ChangeType(Value, type);
        }
        
        public string Represent() {
            return (Convert.ChangeType(Value, type)).ToString();
        }
    }

    public class KVStructS : KVStruct {
        public KVStructS(string defVal = "") : base(defVal) {
            this.type = typeof(String);
            this.Value = string.Empty;
        }
    }

    public class KVStructI : KVStruct {
        public KVStructI(string defVal = "") : base(defVal) => this.type = typeof(Int32);
    }

    public class KVStructF : KVStruct {
        public KVStructF(string defVal = "") : base(defVal) => this.type = typeof(Single);
    }

    public class KVStructB : KVStruct {
        public KVStructB(string defVal = "") : base(defVal) => this.type = typeof(Boolean);
    }

    public interface IDataStruct {

        string GetKeyName();
    }

    public class DataContainer {
        public object data;
        public Type type;

        public static DataContainer Create(object Data) {
            return new DataContainer {
                data = Data,
                type = Data.GetType()
            };
        }
    }


}
