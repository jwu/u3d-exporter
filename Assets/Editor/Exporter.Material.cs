using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using System.IO;

namespace exsdk {
  public partial class Exporter {

    // -----------------------------------------
    // DumpTexture
    // -----------------------------------------

    JSON_Material DumpMaterial(Material _mat) {
      ShaderInfo shdInfo = Utils.GetShaderInfo(_mat);
      JSON_Material result = new JSON_Material();

      result.type = shdInfo.type;

      foreach (var prop in shdInfo.properties) {
        // parse property by type
        if (prop.type == "color") {
          var color = _mat.GetColor(prop.name);
          result.properties.Add(prop.mapping, new float[4] {
            color.r, color.g, color.b, color.a
          });
        } else if (prop.type == "vec4") {
          var vec4 = _mat.GetVector(prop.name);
          result.properties.Add(prop.mapping, new float[4] {
            vec4.x, vec4.y, vec4.z, vec4.w
          });
        } else if (prop.type == "tex2d") {
          var texture = _mat.GetTexture(prop.name);
          var offset = _mat.GetTextureOffset(prop.name);
          var scale = _mat.GetTextureScale(prop.name);
          var textureAsset = Utils.AssetID(texture);

          result.properties.Add(prop.mapping, textureAsset);
          result.properties.Add(prop.mapping + "Tiling", new float[2] {
            scale.x, scale.y
          });
          result.properties.Add(prop.mapping + "Offset", new float[2] {
            offset.x, offset.y
          });
        }
      }

      return result;
    }
  }
}