//This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;

namespace yfxsApp.csfiles
{
    class YfbSmallPackage
    {
        public YfbSmallPackage()
        {
            itemlist = new Dictionary<string, IStorageFile>();
        }
        public YfbSmallPackage(Dictionary<string, IStorageFile> items)
        {
            itemlist = items;
        }
        IDictionary<string, IStorageFile> itemlist;
        public void AddItem(IStorageFile item)
        {
            itemlist.Add(item.Name,item);
        }
        public void AddItem(IStorageFile item,string name)
        {
            itemlist.Add(name, item);
        }
        public void RemoveItem(string name)
        {
            itemlist.Remove(name);
        }
        public string Name
        {
            get; set;
        } = null;
        public async Task SaveToAsync(Stream stream,string type= "smallpackage")
        {
            var dataWriter = new BinaryWriter(stream);
            dataWriter.Write(Encoding.UTF8.GetBytes("yfb "+type+" 1.0.0.0 http://www.yinyue200.com/zh-cn/appList/yfxsrt/yfbfiles.aspx \n"));
            dataWriter.Write(Encoding.UTF8.GetBytes(Name));
            dataWriter.Write(Encoding.UTF8.GetBytes("\n"));
            foreach (var one in itemlist)
            {
                dataWriter.Write(Encoding.UTF8.GetBytes(one.Key));
                dataWriter.Write(Encoding.UTF8.GetBytes("\n"));
                dataWriter.Write((long)0);
                using (var contentstream = await one.Value.OpenReadAsync())
                {
                    dataWriter.Write(contentstream.Size);
                    await contentstream.AsStream().CopyToAsync(stream);
                }
            }
        }
        public static async Task ExtractToDirectoryAsync(IStorageFolder folder, Stream yfbfile)
        {
            string _name;
            using (yfbfile)
            {
                long seek;
                using (var reader = new System.IO.StreamReader(yfbfile, System.Text.Encoding.UTF8, false, 128, true))
                {
                    string a = await reader.ReadLineAsync();
                    var b = a.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (b[0] == "yfb" && b[3] == "http://www.yinyue200.com/zh-cn/appList/yfxsrt/yfbfiles.aspx")
                    {
                        if (b[2] == "1.0.0.0" && b[1] == "smallpackage")
                        {
                            //文件符合格式要求
                            _name = await reader.ReadLineAsync();
                            seek = System.Text.Encoding.UTF8.GetByteCount(a + "\n" + _name + "\n");
                        }
                        else
                        {
                            throw new NotSupportedException();
                        }
                    }
                    else
                    {
                        throw new InvalidDataException();
                    }
                }
                yfbfile.Seek(seek, SeekOrigin.Begin);
                bool nextend = false;
                long nextpos = 0;

                string name;
                List<byte> bytelist = new List<byte>();
                re:
                int byt = yfbfile.ReadByte();
                if (byt == -1)
                {
                    return;
                }
                else if (byt == 10)
                {
                    name = Encoding.UTF8.GetString(bytelist.ToArray());
                }
                else
                {
                    bytelist.Add((byte)byt);
                    goto re;
                }
                byte[] longbuffer = new byte[8];
                await yfbfile.ReadAsync(longbuffer, 0, longbuffer.Length);
                var startpos = BitConverter.ToInt64(longbuffer, 0);
                await yfbfile.ReadAsync(longbuffer, 0, longbuffer.Length);
                ulong startlens = BitConverter.ToUInt64(longbuffer, 0);
                long nowpos = yfbfile.Position;
                if (startpos == 0)
                {

                }
                else
                {
                    var fu = yfbfile.ReadByte();
                    if (fu == 1)
                    {
                        nextend = true;
                    }
                    else if (fu == 0)
                    {
                        await yfbfile.ReadAsync(longbuffer, 0, longbuffer.Length);
                        nextpos = BitConverter.ToInt64(longbuffer, 0);
                    }
                    yfbfile.Seek(startpos + nowpos, SeekOrigin.Begin);
                }
                byte[] content = new byte[startlens];
                yfbfile.Read(content, 0, content.Length);
                using (var str = await (await folder.CreateFileAsync(name.Replace("/", "\\"),CreationCollisionOption.ReplaceExisting)).OpenTransactedWriteAsync())
                {
                    await str.Stream.WriteAsync(content.AsBuffer());
                    await str.CommitAsync();
                }
                if (nextend)
                {
                    return;
                }
                if (nextpos != 0)
                {
                    yfbfile.Seek(nextpos, SeekOrigin.Current);
                }
                bytelist = new List<byte>();
                goto re;
            }
        }
        public static async Task<Tuple<string,Dictionary<string,byte[]>>> LoadToBytesAsync(Stream yfbfile)
        {
            string _name;
            Dictionary<string, byte[]> dic = new Dictionary<string, byte[]>();
            using (yfbfile)
            {
                long seek;
                using (var reader = new System.IO.StreamReader(yfbfile, System.Text.Encoding.UTF8, false, 128, true))
                {
                    string a = await reader.ReadLineAsync();
                    var b = a.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (b[0] == "yfb" && b[3] == "http://www.yinyue200.com/zh-cn/appList/yfxsrt/yfbfiles.aspx")
                    {
                        if (b[2] == "1.0.0.0" && b[1] == "smallpackage")
                        {
                            //文件符合格式要求
                            _name = await reader.ReadLineAsync();
                            seek = System.Text.Encoding.UTF8.GetByteCount(a + "\n" + _name + "\n");
                        }
                        else
                        {
                            throw new NotSupportedException();
                        }
                    }
                    else
                    {
                        throw new InvalidDataException();
                    }
                }
                yfbfile.Seek(seek, SeekOrigin.Begin);
                bool nextend = false;
                long nextpos = 0;

                string name;
                List<byte> bytelist = new List<byte>();
                re:
                int byt = yfbfile.ReadByte();
                if (byt == -1)
                {
                    return new Tuple<string, Dictionary<string, byte[]>>(_name, dic);
                }
                else if (byt == 10)
                {
                    name = Encoding.UTF8.GetString(bytelist.ToArray());
                }
                else
                {
                    bytelist.Add((byte)byt);
                    goto re;
                }
                byte[] longbuffer = new byte[8];
                await yfbfile.ReadAsync(longbuffer, 0, longbuffer.Length);
                var startpos = BitConverter.ToInt64(longbuffer, 0);
                await yfbfile.ReadAsync(longbuffer, 0, longbuffer.Length);
                ulong startlens = BitConverter.ToUInt64(longbuffer, 0);
                long nowpos = yfbfile.Position;
                if (startpos == 0)
                {

                }
                else
                {
                    var fu = yfbfile.ReadByte();
                    if (fu==1)
                    {
                        nextend = true;
                    }
                    else if(fu==0)
                    {
                        await yfbfile.ReadAsync(longbuffer, 0, longbuffer.Length);
                        nextpos = BitConverter.ToInt64(longbuffer, 0);
                    }
                    yfbfile.Seek(startpos + nowpos, SeekOrigin.Begin);
                }
                byte[] content = new byte[startlens];
                yfbfile.Read(content, 0, content.Length);
                dic.Add(name, content);
                if(nextend )
                {
                    return new Tuple<string, Dictionary<string, byte[]>>(_name, dic);
                }
                if (nextpos !=0)
                {
                    yfbfile.Seek(nextpos, SeekOrigin.Current);
                }
                bytelist = new List<byte>();
                goto re;
            }
        }
    }
}
