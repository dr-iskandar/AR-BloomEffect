//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public enum SelectedItem
//{
//    None = 1,
//    Seats = 2,
//    Car = 3
//}
//
//public class ColorChanger : MonoBehaviour {
//
//    public ColorPicker ColorPicker;
//    public Material[] SeatMaterials;
//    public Material[] CarMaterials;
//
//    public static SelectedItem SelectedItem = SelectedItem.None; //will be set from gui
//	
//    // Use this for initialization
//	void Start () {
//        ColorPicker.onValueChanged.AddListener(color =>
//        {
//            if (SelectedItem == SelectedItem.Seats)
//            {
//                foreach (var mat in SeatMaterials) {
//                    mat.color = ColorPicker.CurrentColor;
//                }
//            }
//            else if (SelectedItem == SelectedItem.Car)
//            {
//                foreach (var mat in CarMaterials) {
//                    mat.color = ColorPicker.CurrentColor;
//                }
//            }
//        });
//	}
//	
//}
