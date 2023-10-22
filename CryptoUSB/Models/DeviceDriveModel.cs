/*  
 *  Автор: Миловидов Артур
 *  Время: 22.10.2023 22:20
 *  Статус: ОК - Класс переведен
 */

namespace CryptoUSB.Models
{
    public class DeviceDriveModel
    {
        private string _model;
        private string _path;
        private long _size = 0;

        public DeviceDriveModel(string model, string pathString, long size)
        {
            this._model = model;
            this._path = pathString;
            this._size = size;
        }

        public override string ToString() => $"{this._model} | {this._path} | {this._size}";
        public string GetPath() => this._path;
    }
}
