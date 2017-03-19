//This Source Code Form is subject to the terms of the Mozilla Public
//  License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Threading.Tasks;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace smallpackage打包器
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            bt1.IsEnabled = false;
            try
            {
                FolderPicker fp = new FolderPicker();
                fp.FileTypeFilter.Add("*");
                var folder = await fp.PickSingleFolderAsync();
                if (folder != null)
                {
                    yfxsApp.csfiles.YfbSmallPackage yp = new yfxsApp.csfiles.YfbSmallPackage(await GetChildFiles(string.Empty, folder));
                    yp.Name = ant.Text;
                    FileSavePicker fsp = new FileSavePicker();
                    fsp.FileTypeChoices.Add("yfb", new List<string> { ".yfb" });
                    var sf = await fsp.PickSaveFileAsync();
                    using (var wrs = await sf.OpenStreamForWriteAsync())
                    {
                        wrs.SetLength(0);
                        await yp.SaveToAsync(wrs, ccb.SelectedValue.ToString());
                    }
                }
            }
            finally
            {
                bt1.IsEnabled = true;
            }
        }
        public static async Task<Dictionary<string,IStorageFile>> GetChildFiles(string name,IStorageFolder isf)
        {
            var shuchu = new Dictionary<string, IStorageFile>();
            foreach (IStorageItem si in await isf.GetItemsAsync())
            {
                IStorageFile sf = si as IStorageFile;
                IStorageFolder sfr = si as IStorageFolder;
                if (sf != null)
                {
                    shuchu.Add(name+sf.Name,sf);
                }
                if (sfr != null)
                {
                    foreach (var csf in await GetChildFiles(name + sfr.Name+"\\", sfr))
                    {
                        shuchu.Add(csf.Key,csf.Value);
                    }
                }
            }
            return shuchu;
        }
    }
}
