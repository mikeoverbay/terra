﻿#version 330 compatibility
 
layout (triangles) in;
layout (line_strip) out;
layout (max_vertices = 18) out;
#extension GL_EXT_geometry_shader4 : enable
//
uniform int mode;
uniform float l_length;
 in vec3 n[3];
 in vec3 t[3];
 in vec3 b[3];
 out vec4 color;
void main()
{
    vec4 sumV;
    vec4 sumN;
    if (mode == 1) {
        //Normal
            color = vec4(1.0,0.0,0.0,1.0);
            sumV = (gl_PositionIn[0] + gl_PositionIn[1] + gl_PositionIn[2]) / 3.0;
            sumN.xyz = (n[0].xyz + n[1].xyz + n[2].xyz) / 3.0;
            sumN.w = 0.0;
            gl_Position = gl_ModelViewProjectionMatrix * sumV;
            EmitVertex();
            gl_Position = gl_ModelViewProjectionMatrix * (sumV + (sumN * l_length));
            EmitVertex();
            EndPrimitive();
        //Tangent
            color = vec4(0.0,1.0,0.0,1.0);
            sumV = (gl_PositionIn[0] + gl_PositionIn[1] + gl_PositionIn[2]) / 3.0;
            sumN.xyz = (t[0].xyz + t[1].xyz + t[2].xyz) / 3.0;
            sumN.w = 0.0;
            gl_Position = gl_ModelViewProjectionMatrix * sumV;
            EmitVertex();
            gl_Position = gl_ModelViewProjectionMatrix * (sumV + (sumN * l_length));
            EmitVertex();
            EndPrimitive();
        //biTangent
            color = vec4(0.0,0.0,1.0,1.0);
            sumV = (gl_PositionIn[0] + gl_PositionIn[1] + gl_PositionIn[2]) / 3.0;
            sumN.xyz = (b[0].xyz + b[1].xyz + b[2].xyz) / 3.0;
            sumN.w = 0.0;
            gl_Position = gl_ModelViewProjectionMatrix * sumV;
            EmitVertex();
            gl_Position = gl_ModelViewProjectionMatrix * (sumV + (sumN * l_length));
            EmitVertex();
            EndPrimitive();
    }
    else
    {
        // normal
        color = vec4(1.0,0.0,0.0,1.0);
        for(int i = 0; i < gl_VerticesIn; ++i)
        {
            gl_Position = gl_ModelViewProjectionMatrix * gl_PositionIn[i];
            EmitVertex();
            gl_Position = gl_ModelViewProjectionMatrix * (gl_PositionIn[i] + (vec4(n[i], 0) * l_length));
            EmitVertex();
            EndPrimitive();
        }
        // Tangent
        color = vec4(0.0,1.0,0.0,1.0);
        for(int i = 0; i < gl_VerticesIn; ++i)
        {
            gl_Position = gl_ModelViewProjectionMatrix * gl_PositionIn[i];
            EmitVertex();
            gl_Position = gl_ModelViewProjectionMatrix * (gl_PositionIn[i] + (vec4(t[i], 0) * l_length));
            EmitVertex();
            EndPrimitive();
        }
        // biTangent
            color = vec4(0.0,0.0,1.0,1.0);
        for(int i = 0; i < gl_VerticesIn; ++i)
        {
            gl_Position = gl_ModelViewProjectionMatrix * gl_PositionIn[i];
            EmitVertex();
            gl_Position = gl_ModelViewProjectionMatrix * (gl_PositionIn[i] + (vec4(b[i], 0) * l_length));
            EmitVertex();
            EndPrimitive();
        }
    } // mode
} // main