using System.Windows;
using InTheHand.Net.Sockets;
using System;
using InTheHand.Net;
using System.Collections.Generic;
using System.Linq;
using InTheHand.Net.Bluetooth;

// I know that this can all be so much better, but this is a tinkering project, I made it to learn from it...
// Later on I might make this decently coded, but I'm not sure yet...
//Author: Giovanni Allemeersch

namespace Bluetooth_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BluetoothDeviceInfo[] devices;
        List<BluetoothDeviceInfo> bluetoothDeviceInfos = new List<BluetoothDeviceInfo>();
        BluetoothEndPoint ble;
        public MainWindow()
        {
            InitializeComponent();
            BluetoothClient device = new BluetoothClient();
            device.BeginDiscoverDevices(8, true, true, true, true, ScanDevices, device);
            
        }

        /// <summary>
        /// Method to scan for the devices present
        /// </summary>
        /// <param name="result"></param>
        private void ScanDevices(IAsyncResult result)
        {
            //Get back the BluetoothClient which was passed as the state parameter in BeginDiscoverDevices
            BluetoothClient thisDevice = result.AsyncState as BluetoothClient;
            //Check if the scanning is completed
            if (result.IsCompleted)
            {
                //get the list of devices
               devices = thisDevice.EndDiscoverDevices(result);
                 //add the devices to the listbox
                foreach(BluetoothDeviceInfo b in devices)
                {
                    this.Dispatcher.Invoke(() => {
                        lbItems.Items.Add(b.DeviceName.ToString());
                        bluetoothDeviceInfos.Add(b);
                    });
                }
            }
        }

        /// <summary>
        /// Event handler for the selection changed event in the listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbItems_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var b = from a in bluetoothDeviceInfos where a.DeviceName == lbItems.SelectedItem.ToString() select a;
            
            foreach(var c in b)
            {
                bool test = BluetoothSecurity.PairRequest(c.DeviceAddress, "01234");
                if (test)
                    MessageBox.Show("connected");
                else
                    MessageBox.Show("could not connect :(");
            }
            //BluetoothClient blc = new BluetoothClient();

       
        }
    }
}
