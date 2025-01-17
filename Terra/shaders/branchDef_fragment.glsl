﻿// trees_fragment.txt
// used to render trees
#version 330 compatibility

layout (location = 0) out vec4 gColor;
layout (location = 1) out vec4 gNormal;
layout (location = 2) out vec4 gPosition;
layout (location = 3) out float gFlag;

uniform sampler2D normalMap;
uniform sampler2D colorMap;

in vec2 texCoord;
in vec3 n;
in vec4 Vertex;
in mat3 TBN;
void main (void)
{
  //discard;
  vec4 bump= texture2D(normalMap, texCoord)*2.0-1.0;
  bump.y *= -1.0;
  bump.a = texture2D(normalMap, texCoord).a;
  bump.xyz =  normalize(TBN * bump.xyz);

  gColor = texture2D(colorMap, texCoord);
  gNormal.xyz = normalize(n.xyz + bump.xyz)*0.5+0.5;
  gNormal.w = bump.a;
  gPosition = Vertex;
  gFlag = (192.0/255.0);
  }
