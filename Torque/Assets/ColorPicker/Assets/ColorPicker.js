#pragma strict
import System;
import System.IO;

var _red : UnityEngine.UI.Slider;
var _green : UnityEngine.UI.Slider;
var _blue : UnityEngine.UI.Slider;

var newColor : Color32;

var matsToColor : Material[];

var curObj : int;;

public function inform (number : int) {

	curObj = number;
}

function Update () {

	newColor = new Color32(_red.value, _green.value, _blue.value, 255);
	gameObject.GetComponent(UnityEngine.UI.Image).color = newColor;
	
	ApplyColor();
}

function ApplyColor () {
	
	matsToColor[curObj].color = newColor;
}


