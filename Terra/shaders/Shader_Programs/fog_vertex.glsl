﻿// render_vertex.txt
// used to render blended hirez terrain


uniform int main_texture;
uniform vec3 cam_position;

uniform float tile_width;
uniform float mapHeight;

varying vec3 lightVec; 
varying vec3 eyeVec;
varying vec2 texCoord;
varying vec3 g_viewvector;
varying vec3 lightDirection;

varying vec2 fog_uv_coords;

varying vec3 n;
varying float ln;
varying vec4 Vposition;
varying vec4 mask;
varying vec4 mask_2;
varying mat3 TBN;
varying vec2 hUV;
varying float y_pos;
varying vec3 lightPos;
varying vec3 v_pos;
void main(void)
{ 
    gl_Position = ftransform();     
    gl_TexCoord[0] = -gl_MultiTexCoord0;
    texCoord = gl_MultiTexCoord0.xy/1.001;
    fog_uv_coords = gl_MultiTexCoord3.xy;
  
    vec4 point = gl_Vertex;
    Vposition.x = point.x ;
    Vposition.y = point.y;
    Vposition.z = -point.z;
    Vposition.w = point.w;


    vec3 b,t;

    n   = gl_NormalMatrix * gl_Normal;
    t   = gl_NormalMatrix * gl_MultiTexCoord1.xyz;
    b   = gl_NormalMatrix *gl_MultiTexCoord2.xyz;
    t   = normalize(t - dot(n, t) * n);
    b   = normalize(t - dot(n, t) * n);
    TBN = mat3(t, b, n);

    hUV = -gl_MultiTexCoord0.xy /10.0 ;
    vec4 H = vec4(0.0,mapHeight,0.0,1.0);
    H = gl_ModelViewMatrix * H;
    vec4 vert = gl_ModelViewMatrix * gl_Vertex;
    v_pos.xyz      = vert.xyz;
    lightDirection = gl_LightSource[0].position.xyz - vert.xyz;
     lightPos = gl_LightSource[0].position.xyz;   
    y_pos = mapHeight*.5 ;

    g_viewvector = cam_position - gl_Vertex.xyz;

vec3 tangent;
// Create the mask. 
float c_scale = 1.0; // trial and error... These are best fit values.
float m_scale = 1.0;
    mask = vec4( c_scale ,c_scale , c_scale, c_scale);
    mask_2 = vec4(1.0 ,1.0 ,1.0 ,1.0);

if (main_texture == 1 )
{
mask.r = m_scale;
mask_2.r = 0.0;
}
if (main_texture == 2 )
{
mask.g = m_scale;
mask_2.g = 0.0;
}
if (main_texture == 3 )
{
mask.b = m_scale;
mask_2.b = 0.0;
}
if (main_texture == 4 )
{
mask.a = m_scale;
mask_2.a = 0.0;
}


// This is the cut off distance for bumpping the surface.
ln = distance(point.xyz,cam_position.xyz);
if (ln<400.0)
{
    ln = 1.0 - ln/400.0;
    }
    else
    {
    ln = 0.0;
}
}
