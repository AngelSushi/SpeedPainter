using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BrushDebuggerWindow : EditorWindow {
    [MenuItem("Window/Brush Debugger")]
    public static void ShowWindow() {
        GetWindow<BrushDebuggerWindow>("Brush Debugger");
    }

    private int _textureWidth = 256;
    private int _textureHeight = 256;
    private enum BrushType {
        PENCIL,
        SPRAY,
        CIRCLE
    }
    
    private BrushType _brushType = BrushType.PENCIL;
    private BrushType _lastBrushType;

    private Texture2D _brushTexture;

    private void OnGUI() {
        GUILayout.Label("Texture Settings", EditorStyles.boldLabel);
        _textureWidth = EditorGUILayout.IntField("Width", _textureWidth);
        _textureHeight = EditorGUILayout.IntField("Height", _textureHeight);
        _brushType = (BrushType)EditorGUILayout.EnumPopup("Brush Type", _brushType);
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("Generate Texture")) {
            switch (_brushType) {
                case BrushType.PENCIL:
                    GeneratePencilTexture();
                    break;
                
                case BrushType.SPRAY:
                    GenerateSprayTexture();
                    break;
                
                case BrushType.CIRCLE:
                    GenerateCircleTexture();
                    break;
            }
        }
        
        if(_lastBrushType != _brushType) {
            _brushTexture = null;
        }

        if (_brushTexture) {
            GUILayout.Label("Pencil Texture Preview", EditorStyles.boldLabel);
            GUILayout.Space(10);
            Rect textureRect = GUILayoutUtility.GetRect(256, 256);
            GUI.DrawTexture(textureRect, _brushTexture, ScaleMode.ScaleToFit);
        }

        _lastBrushType = _brushType;
    }

    private void GeneratePencilTexture() {
        Pencil pencil = new Pencil(_textureWidth);
        
        _brushTexture = new Texture2D(_textureWidth, _textureHeight);
        
        pencil.ComputePixels();

        _brushTexture.SetPixels(pencil.Pixels.ToArray());
        _brushTexture.Apply(false);
    }

    private void GenerateSprayTexture() {
        int brushSize = 128;
        Spray spray = new Spray(brushSize);

        _brushTexture = new Texture2D(_textureWidth, _textureHeight);
    
        spray.ComputePixels();

        int startX = (_textureWidth - brushSize) / 2;
        int startY = (_textureHeight - brushSize) / 2;
        
        Color[] texturePixels = _brushTexture.GetPixels(startX, startY, brushSize, brushSize);

        // Get If the current pixel is not transparent, we take the color from the spray
        for (int i = 0; i < brushSize * brushSize; i++) {
            if (spray.Pixels[i] != Color.clear) { 
                texturePixels[i] = spray.Pixels[i];
            }
        }
        
        _brushTexture.SetPixels(startX, startY, brushSize, brushSize, texturePixels);
        _brushTexture.Apply(false);
    }

    
    private void GenerateCircleTexture() {
        Circle circle = new Circle(_textureWidth);
        
        _brushTexture = new Texture2D(_textureWidth, _textureHeight);
        
        circle.ComputePixels();

        _brushTexture.SetPixels(circle.Pixels.ToArray());
        _brushTexture.Apply(false);
    }
}
