﻿#Region "imports"
Imports Tao.OpenGl
Imports Tao.Platform.Windows
Imports Tao.FreeGlut
Imports Tao.FreeGlut.Glut
Imports Tao.DevIl
Imports System.IO
Imports System.Windows
Imports System.Math


#End Region

Module modTextures
    Public max_texture_units As Integer
    Public shadowMapID, coMapID, coMapID2, utility_texture, depthID, fboID, fboID2 As Integer

    Public Function build_layer_normal_texture(ByVal map As Int32, ByVal ms As MemoryStream, ByRef layer As Integer) As Integer
        Dim s As String = ""
        s = Gl.glGetError

        Dim app_local As String = Application.StartupPath.ToString

        Dim texID As UInt32
        ms.Position = 0
        Dim textIn(ms.Length) As Byte
        ms.Read(textIn, 0, ms.Length)
        texID = Ilu.iluGenImage() ' Generation of one image name
        Il.ilBindImage(texID) ' Binding of image name 
        Dim success = Il.ilGetError
        Il.ilLoadL(Il.IL_DDS, textIn, textIn.Length)
        success = Il.ilGetError
        If success = Il.IL_NO_ERROR Then
            Dim width As Integer = Il.ilGetInteger(Il.IL_IMAGE_WIDTH)
            Dim height As Integer = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT)

            success = Il.ilConvertImage(Il.IL_RGB, Il.IL_UNSIGNED_BYTE) ' Convert every colour component into unsigned bytes
            Ilu.iluCrop(64, 64, 0, 896, 896, 1)
            If width > 512 And width < 4096 Then
                'Dim delta = map_layers(map).layers(layer).u * 10.0
                'If delta = 0 Then
                '	Stop
                'End If
                'Ilu.iluScale(width * delta, height * delta, 1)
            End If
            'Ilu.iluFlipImage()
            'Ilu.iluMirror()
            'If your image contains alpha channel you can replace IL_RGB with IL_RGBA 
            Dim text_id As Integer
            Gl.glGenTextures(1, text_id)
            Gl.glEnable(Gl.GL_TEXTURE_2D)
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, text_id)

            If largestAnsio > 0 Then
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAX_ANISOTROPY_EXT, largestAnsio)
            End If


            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR_MIPMAP_LINEAR)
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR)

            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_LOD_BIAS, 0.05)

            'Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAX_LEVEL, 5)

            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_GENERATE_MIPMAP, Gl.GL_TRUE)

            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Il.ilGetInteger(Il.IL_IMAGE_BPP), Il.ilGetInteger(Il.IL_IMAGE_WIDTH), _
            Il.ilGetInteger(Il.IL_IMAGE_HEIGHT), 0, Il.ilGetInteger(Il.IL_IMAGE_FORMAT), Gl.GL_UNSIGNED_BYTE, _
            Il.ilGetData()) '  Texture specification 
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0)
            Il.ilBindImage(0)
            Ilu.iluDeleteImage(texID)
            Return text_id
        Else
            'Stop
            frmMapInfo.I__General_Info_tb.Text += "Failed to load normal texture. Map:" + map.ToString + vbCrLf
        End If
        Il.ilBindImage(0)
        Ilu.iluDeleteImage(texID)
        ms.Close()
        ms.Dispose()
        Return 0
    End Function
    Public Function build_layer_color_texture(ByVal map As Int32, ByVal ms As MemoryStream, ByRef layer As Integer) As Integer
        Dim s As String = ""
        s = Gl.glGetError

        Dim app_local As String = Application.StartupPath.ToString

        Dim texID As UInt32
        ms.Position = 0
        Dim textIn(ms.Length) As Byte
        ms.Read(textIn, 0, ms.Length)
        texID = Ilu.iluGenImage() ' Generation of one image name
        Il.ilBindImage(texID) ' Binding of image name 
        Dim success = Il.ilGetError
        Il.ilLoadL(Il.IL_DDS, textIn, textIn.Length)
        success = Il.ilGetError
        If success = Il.IL_NO_ERROR Then
            Dim width As Integer = Il.ilGetInteger(Il.IL_IMAGE_WIDTH)
            Dim height As Integer = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT)
            ' Convert every colour component into unsigned bytes
            success = Il.ilConvertImage(Il.IL_RGBA, Il.IL_UNSIGNED_BYTE)
            Ilu.iluCrop(64, 64, 0, 896, 896, 1)
            'Ilu.iluFlipImage()
            'Ilu.iluMirror()

            Dim text_id As Integer
            Gl.glGenTextures(1, text_id)

            Gl.glEnable(Gl.GL_TEXTURE_2D)
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, text_id)

            If largestAnsio > 0 Then
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAX_ANISOTROPY_EXT, largestAnsio)
            End If
            Dim e = Gl.glGetError

            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR_MIPMAP_LINEAR)
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR)

            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_LOD_BIAS, 0.05)

            'Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAX_LEVEL, 5)


            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_GENERATE_MIPMAP, Gl.GL_TRUE)

            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Il.ilGetInteger(Il.IL_IMAGE_BPP), Il.ilGetInteger(Il.IL_IMAGE_WIDTH), _
            Il.ilGetInteger(Il.IL_IMAGE_HEIGHT), 0, Il.ilGetInteger(Il.IL_IMAGE_FORMAT), Gl.GL_UNSIGNED_BYTE, _
            Il.ilGetData()) '  Texture specification 
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0)
            Il.ilBindImage(0)
            Ilu.iluDeleteImage(texID)
            e = Gl.glGetError
            Return text_id
        Else

        End If
        Il.ilBindImage(0)
        Ilu.iluDeleteImage(texID)
        ms.Close()
        ms.Dispose()
        Return 0
    End Function

    Public Function get_main_tex_texture_id(ByVal ms As MemoryStream, ByVal w As Integer) As Integer
        Dim s As String = ""
        s = Gl.glGetError
        Dim texID As UInt32
        ms.Position = 0
        Dim textIn(ms.Length) As Byte
        ms.Read(textIn, 0, ms.Length)
        texID = Ilu.iluGenImage() ' Generation of one image name
        Il.ilBindImage(texID) ' Binding of image name 
        Il.ilLoadL(Il.IL_DDS, textIn, textIn.Length)
        Dim success = Il.ilGetError
        success = Il.ilGetError
        ms.Close()
        ms.Dispose()
        'GC.Collect()
        'GC.WaitForFullGCComplete()
        Dim max_texture_size As Integer
        Gl.glGetIntegerv(Gl.GL_MAX_TEXTURE_SIZE, max_texture_size)
        Dim resize As Integer = 512
        Dim mod_ As Integer = (Sqrt(maplist.Length - 1)) And 1
        w += mod_
        If w * 512 > 4096 Then
            resize = CInt(4096 / w)
        End If
        'w -= mod_
        If success = Il.IL_NO_ERROR Then
            Dim width As Integer = Il.ilGetInteger(Il.IL_IMAGE_WIDTH)
            Dim height As Integer = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT)

            Dim passed = Il.ilConvertImage(Il.IL_BGRA, Il.IL_UNSIGNED_BYTE) ' Convert every colour component into unsigned bytes
            If Not passed Then
                MsgBox("Failed Converting Image : get_main_tex_bmp:")
            End If
            'Ilu.iluFlipImage()
            'Ilu.iluMirror()
            Ilu.iluScale(resize * w, resize * w, 1)
            'If your image contains alpha channel you can replace IL_RGB with IL_RGBA 
            Dim image As Int32
            Gl.glGenTextures(1, image)
            Gl.glActiveTexture(Gl.GL_TEXTURE0)
            Gl.glEnable(Gl.GL_TEXTURE_2D)
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, image)
            '**************************************************************************
            If largestAnsio > 0 Then
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAX_ANISOTROPY_EXT, largestAnsio)
            End If

            '		DO NOT CHANGE THESE IDIOT!!!
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST)
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST)
            'Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_GENERATE_MIPMAP, Gl.GL_TRUE)

            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Il.ilGetInteger(Il.IL_IMAGE_BPP), Il.ilGetInteger(Il.IL_IMAGE_WIDTH), _
            Il.ilGetInteger(Il.IL_IMAGE_HEIGHT), 0, Il.ilGetInteger(Il.IL_IMAGE_FORMAT), Gl.GL_UNSIGNED_BYTE, _
            Il.ilGetData()) '  Texture specification 
            Gl.glFinish()

            'frmMain.pb2.visible = True
            'frmMain.pb2.SendToBack()
            frmMain.pb2.Height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT)
            frmMain.pb2.Width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH)
            Il.ilBindImage(0)
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0)
            Il.ilDeleteImages(1, texID)
            'GC.Collect()
            'GC.WaitForFullGCComplete()
            ' has to run on vertical and horz
            Gl.glBindFramebufferEXT(Gl.GL_FRAMEBUFFER_EXT, 0)

            If Not (Wgl.wglMakeCurrent(pb2_hDC, pb2_hRC)) Then
                MessageBox.Show("Unable to make rendering context current")
                End
            End If

            image = blur_image(image, "vert", False)
            image = blur_image(image, "horz", True)

            If Not (Wgl.wglMakeCurrent(pb1_hDC, pb1_hRC)) Then
                MessageBox.Show("Unable to make rendering context current")
                End
            End If

            Return image
        Else
            MsgBox("Failed at :get_main_tex_bmp:" + success.ToString)
        End If
        Return -1
    End Function

    Public Function build_textures(ByVal map As Int32, ByVal ms As MemoryStream) As Bitmap
        Dim s As String = ""
        s = Gl.glGetError

        Dim app_local As String = Application.StartupPath.ToString

        Dim texID As UInt32
        ms.Position = 0
        Dim textIn(ms.Length) As Byte
        ms.Read(textIn, 0, ms.Length)
        Dim bmap As New System.Drawing.Bitmap(10, 10, PixelFormat.Format32bppArgb)
        texID = Ilu.iluGenImage() ' Generation of one image name
        Il.ilBindImage(texID) ' Binding of image name 
        Dim success = Il.ilGetError
        Il.ilLoadL(Il.IL_DDS, textIn, textIn.Length)
        success = Il.ilGetError
        If success = Il.IL_NO_ERROR Then
            Dim width As Integer = Il.ilGetInteger(Il.IL_IMAGE_WIDTH)
            Dim height As Integer = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT)

            ' Create the bitmap.
            Dim bitmap = New System.Drawing.Bitmap(width, height, PixelFormat.Format24bppRgb)
            Dim rect As Rectangle = New Rectangle(0, 0, width, height)

            ' Store the DevIL image data into the bitmap.
            Dim bitmapData As BitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb)

            success = Il.ilConvertImage(Il.IL_BMP, Il.IL_UNSIGNED_BYTE)

            Il.ilCopyPixels(0, 0, 0, width, height, 1, Il.IL_BGR, Il.IL_UNSIGNED_BYTE, bitmapData.Scan0)
            bitmap.UnlockBits(bitmapData)



            Dim er As Integer = Gl.glGetError

            'success = Il.ilConvertImage(Il.IL_RGBA, Il.IL_UNSIGNED_BYTE) ' Convert every colour component into unsigned bytes
            Ilu.iluFlipImage()
            Ilu.iluMirror()
            'If your image contains alpha channel you can replace IL_RGB with IL_RGBA 

            Gl.glGenTextures(1, maplist(map).colorMapId)
            Gl.glEnable(Gl.GL_TEXTURE_2D)
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, maplist(map).colorMapId)

            If largestAnsio > 0 Then
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAX_ANISOTROPY_EXT, largestAnsio)
            End If


            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR)
            'Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_GENERATE_MIPMAP, Gl.GL_TRUE)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_CLAMP_TO_EDGE)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_CLAMP_TO_EDGE)

            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Il.ilGetInteger(Il.IL_IMAGE_BPP), Il.ilGetInteger(Il.IL_IMAGE_WIDTH), _
            Il.ilGetInteger(Il.IL_IMAGE_HEIGHT), 0, Il.ilGetInteger(Il.IL_IMAGE_FORMAT), Gl.GL_UNSIGNED_BYTE, _
            Il.ilGetData()) '  Texture specification 
            Gl.glGenerateMipmapEXT(Gl.GL_TEXTURE_2D)
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0)
            Il.ilBindImage(0)
            Ilu.iluDeleteImage(texID)
            bmap = bitmap.Clone
            bitmap.Dispose()
        Else
            Stop
        End If
        ms.Close()
        ms.Dispose()
        Return bmap
    End Function
    Public Function get_tex_id_from_bmp(b As Bitmap)
        b.RotateFlip(RotateFlipType.RotateNoneFlipXY)
        Dim tex_id As Integer
        Gl.glGenTextures(1, tex_id)
        Dim bitmapData = b.LockBits(New Rectangle(0, 0, b.Width, _
                             b.Height), Imaging.ImageLockMode.ReadOnly, Imaging.PixelFormat.Format32bppArgb)
        Gl.glBindTexture(Gl.GL_TEXTURE_2D, tex_id)

        Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR)
        Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR)
        Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_CLAMP_TO_EDGE)
        Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_CLAMP_TO_EDGE)
        Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, 4, b.Width, b.Height, 0, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0)
        Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0)
        b.UnlockBits(bitmapData) ' Unlock The Pixel Data From Memory
        b.Dispose()
        'Application.DoEvents()
        Return tex_id
    End Function
    Public Function make_dummy_texture() As Integer
        Dim dummy As Integer
        Gl.glGenTextures(1, dummy)
        Dim b As New Bitmap(2, 2, Imaging.PixelFormat.Format32bppArgb)
        Dim g As Graphics = Graphics.FromImage(b)
        g.Clear(Color.Black)
        Dim bitmapData = b.LockBits(New Rectangle(0, 0, 2, _
                             2), Imaging.ImageLockMode.ReadOnly, Imaging.PixelFormat.Format32bppArgb)
        Gl.glBindTexture(Gl.GL_TEXTURE_2D, dummy)

        Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST)
        Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST)
        Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, 4, b.Width, b.Height, 0, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0)
        b.UnlockBits(bitmapData) ' Unlock The Pixel Data From Memory
        Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0)
        b.Dispose()
        g.Dispose()
        GC.Collect()
        Return dummy
    End Function

    Public Function get_texture(ByRef ms As MemoryStream, ByVal shrink As Boolean) As Integer
        'Dim s As String = ""
        's = Gl.glGetError
        'Return 1
        Dim image_id As Integer = -1
        'Dim app_local As String = Application.StartupPath.ToString
        Dim texID As UInt32
        ms.Position = 0
        Dim textIn(ms.Length) As Byte
        ms.Read(textIn, 0, ms.Length)
        texID = Ilu.iluGenImage() ' /* Generation of one image name */
        Il.ilBindImage(texID) '; /* Binding of image name */
        Dim success = Il.ilGetError
        Dim c = 0
        While success <> Il.IL_NO_ERROR
            success = Il.ilGetError
            c += 1
        End While

        Il.ilLoadL(Il.IL_DDS, textIn, textIn.Length)
        success = Il.ilGetError
        If success = Il.IL_NO_ERROR Then
            'Ilu.iluFlipImage()
            Ilu.iluMirror()
            Dim width As Integer = Il.ilGetInteger(Il.IL_IMAGE_WIDTH)
            Dim height As Integer = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT)


            Il.ilConvertImage(Il.IL_BGRA, Il.IL_UNSIGNED_BYTE)
            If shrink Then
                Ilu.iluScale(width / 2.0, height / 2.0, 1)
                width *= 0.5 : height *= 0.5
            End If
            Dim er As Integer = Gl.glGetError

            success = Il.ilConvertImage(Il.IL_RGBA, Il.IL_UNSIGNED_BYTE)

            Gl.glGenTextures(1, image_id)
            Gl.glEnable(Gl.GL_TEXTURE_2D)
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, image_id)

            If largestAnsio > 0 Then
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAX_ANISOTROPY_EXT, largestAnsio)
            End If


            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST_MIPMAP_LINEAR)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_GENERATE_MIPMAP, Gl.GL_TRUE)

            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT)

            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Il.ilGetInteger(Il.IL_IMAGE_BPP), Il.ilGetInteger(Il.IL_IMAGE_WIDTH), _
            Il.ilGetInteger(Il.IL_IMAGE_HEIGHT), 0, Il.ilGetInteger(Il.IL_IMAGE_FORMAT), Gl.GL_UNSIGNED_BYTE, _
            Il.ilGetData()) '  Texture specification 
            Gl.glFinish()
            success = Il.ilGetError
        Else
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0)
            Il.ilBindImage(0)
            Ilu.iluDeleteImage(texID)
            'File.WriteAllBytes("c:\!_dds_dump\dump.dds", textIn)
            ms.Close()
            ms.Dispose()
            Return 37
            MsgBox("Out of memory error at :get texture:", MsgBoxStyle.Critical, "Well Shit!")
            End
        End If
        'ReDim textIn(0)
        'File.WriteAllBytes("c:\!_dds_dump\dump.dds", textIn)
        Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0)
        Il.ilBindImage(0)
        Ilu.iluDeleteImage(texID)
        ms.Close()
        ms.Dispose()
        'GC.Collect()
        Return image_id
    End Function
    Public Function get_tree_texture(ByRef ms As MemoryStream, ByVal shrink As Boolean) As Integer
        'Dim s As String = ""
        's = Gl.glGetError
        Dim image_id As Integer = -1
        'Dim app_local As String = Application.StartupPath.ToString

        Dim texID As UInt32
        ms.Position = 0
        Dim textIn(ms.Length) As Byte
        ms.Read(textIn, 0, ms.Length)
        texID = Ilu.iluGenImage() ' /* Generation of one image name */
        Il.ilBindImage(texID) '; /* Binding of image name */
        Dim success = Il.ilGetError
        Il.ilLoadL(Il.IL_DDS, textIn, textIn.Length)
        success = Il.ilGetError
        If success = Il.IL_NO_ERROR Then
            'Ilu.iluFlipImage()
            Ilu.iluMirror()
            Dim width As Integer = Il.ilGetInteger(Il.IL_IMAGE_WIDTH)
            Dim height As Integer = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT)


            Il.ilConvertImage(Il.IL_BGRA, Il.IL_UNSIGNED_BYTE)
            If shrink Then
                If frmMain.m_low_quality_textures.Checked Then
                    If width > 256 Then
                        Dim delta = 256 / width
                        Ilu.iluScale(width * delta, height * delta, 1)
                    End If
                Else
                    If width > 512 Then
                        Dim delta = 512 / width
                        'Ilu.iluScale(width * delta, height * delta, 1)
                    End If
                End If
            End If
            Dim er As Integer = Gl.glGetError

            success = Il.ilConvertImage(Il.IL_RGBA, Il.IL_UNSIGNED_BYTE) ' Convert every colour component into unsigned bytes
            'If your image contains alpha channel you can replace IL_RGB with IL_RGBA */
            Gl.glGenTextures(1, image_id)
            Gl.glEnable(Gl.GL_TEXTURE_2D)
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, image_id)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST_MIPMAP_LINEAR)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_GENERATE_MIPMAP, Gl.GL_TRUE)

            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT)

            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Il.ilGetInteger(Il.IL_IMAGE_BPP), Il.ilGetInteger(Il.IL_IMAGE_WIDTH), _
            Il.ilGetInteger(Il.IL_IMAGE_HEIGHT), 0, Il.ilGetInteger(Il.IL_IMAGE_FORMAT), Gl.GL_UNSIGNED_BYTE, _
            Il.ilGetData()) '  Texture specification 
            Gl.glFinish()
        Else
            MsgBox("failed to create Texture", MsgBoxStyle.Exclamation, "Damn..")
        End If
        ReDim textIn(0)
        Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0)
        Il.ilBindImage(0)
        Ilu.iluDeleteImage(texID)
        ms.Close()
        ms.Dispose()
        'GC.Collect()
        Return image_id
    End Function
    Public Function get_normal_texture(ByRef ms As MemoryStream, ByVal shrink As Boolean) As Integer
        'Dim s As String = ""
        's = Gl.glGetError
        'Return 1
        Dim image_id As Integer = -1
        'Dim app_local As String = Application.StartupPath.ToString

        Dim texID As UInt32
        ms.Position = 0
        Dim textIn(ms.Length) As Byte
        ms.Read(textIn, 0, ms.Length)
        texID = Ilu.iluGenImage() ' /* Generation of one image name */
        Il.ilBindImage(texID) '; /* Binding of image name */
        Dim success = Il.ilGetError
        Il.ilLoadL(Il.IL_DDS, textIn, textIn.Length)
        success = Il.ilGetError
        If success = Il.IL_NO_ERROR Then
            'Ilu.iluFlipImage()
            Ilu.iluMirror()
            Dim width As Integer = Il.ilGetInteger(Il.IL_IMAGE_WIDTH)
            Dim height As Integer = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT)
            If shrink Then
                Ilu.iluScale(width / 2.0, height / 2.0, 1)
                width *= 0.5 : height *= 0.5
            End If

            success = Il.ilConvertImage(Il.IL_RGBA, Il.IL_UNSIGNED_BYTE) ' Convert every colour component into unsigned bytes
            'If your image contains alpha channel you can replace IL_RGB with IL_RGBA */
            Gl.glGenTextures(1, image_id)
            Gl.glEnable(Gl.GL_TEXTURE_2D)
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, image_id)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST_MIPMAP_LINEAR)

            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_GENERATE_MIPMAP, Gl.GL_TRUE)

            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT)

            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Il.ilGetInteger(Il.IL_IMAGE_BPP), Il.ilGetInteger(Il.IL_IMAGE_WIDTH), _
            Il.ilGetInteger(Il.IL_IMAGE_HEIGHT), 0, Il.ilGetInteger(Il.IL_IMAGE_FORMAT), Gl.GL_UNSIGNED_BYTE, _
            Il.ilGetData()) '  Texture specification 

            Gl.glFinish()
        Else
            Return 1
            MsgBox("failed to create normalMap Texture", MsgBoxStyle.Exclamation, "Damn..")
        End If
        ReDim textIn(0)
        Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0)
        Il.ilBindImage(0)
        Ilu.iluDeleteImage(texID)
        ms.Close()
        ms.Dispose()
        Return image_id
    End Function
    Public Function get_texture_no_alpha(ByRef ms As MemoryStream, ByVal shrink As Boolean) As Integer
        'Dim s As String = ""
        's = Gl.glGetError
        Dim image_id As Integer = -1
        'Dim app_local As String = Application.StartupPath.ToString

        Dim texID As UInt32
        ms.Position = 0
        Dim textIn(ms.Length) As Byte
        ms.Read(textIn, 0, ms.Length)
        texID = Ilu.iluGenImage() ' /* Generation of one image name */
        Il.ilBindImage(texID) '; /* Binding of image name */
        Dim success = Il.ilGetError
        Il.ilLoadL(Il.IL_DDS, textIn, textIn.Length)
        success = Il.ilGetError
        If success = Il.IL_NO_ERROR Then
            'Ilu.iluFlipImage()
            Ilu.iluMirror()
            Dim width As Integer = Il.ilGetInteger(Il.IL_IMAGE_WIDTH)
            Dim height As Integer = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT)

            Il.ilConvertImage(Il.IL_BGRA, Il.IL_UNSIGNED_BYTE)
            If shrink Then
                If shrink Then
                    Ilu.iluScale(width / 2.0, height / 2.0, 1)
                    width *= 0.5 : height *= 0.5
                End If
            End If
            Dim er As Integer = Gl.glGetError

            success = Il.ilConvertImage(Il.IL_RGB, Il.IL_UNSIGNED_BYTE) ' Convert every colour component into unsigned bytes
            'If your image contains alpha channel you can replace IL_RGB with IL_RGBA */
            Gl.glGenTextures(1, image_id)
            Gl.glEnable(Gl.GL_TEXTURE_2D)
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, image_id)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST_MIPMAP_LINEAR)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_GENERATE_MIPMAP, Gl.GL_TRUE)

            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT)

            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Il.ilGetInteger(Il.IL_IMAGE_BPP), Il.ilGetInteger(Il.IL_IMAGE_WIDTH), _
            Il.ilGetInteger(Il.IL_IMAGE_HEIGHT), 0, Il.ilGetInteger(Il.IL_IMAGE_FORMAT), Gl.GL_UNSIGNED_BYTE, _
            Il.ilGetData()) '  Texture specification 

            Gl.glFinish()
        Else
            MsgBox("failed to create no_Alpha Texture", MsgBoxStyle.Exclamation, "Damn..")
        End If
        ReDim textIn(0)
        Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0)
        Il.ilBindImage(0)
        Ilu.iluDeleteImage(texID)
        ms.Close()
        ms.Dispose()
        'GC.Collect()
        Return image_id
    End Function
    Public Function Load_DDS_File(path As String) As Integer
        'Dim s As String = ""
        's = Gl.glGetError
        Dim image_id As Integer = -1
        'Dim app_local As String = Application.StartupPath.ToString

        Dim texID As UInt32
        texID = Ilu.iluGenImage() ' /* Generation of one image name */
        Il.ilBindImage(texID) '; /* Binding of image name */
        Dim success As Integer = Il.ilGetError

        Il.ilLoad(Il.IL_DDS, path)
        success = Il.ilGetError

        If success = Il.IL_NO_ERROR Then
            'Ilu.iluFlipImage()
            Ilu.iluMirror()
            Dim width As Integer = Il.ilGetInteger(Il.IL_IMAGE_WIDTH)
            Dim height As Integer = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT)

            Il.ilConvertImage(Il.IL_BGRA, Il.IL_UNSIGNED_BYTE)

            success = Il.ilConvertImage(Il.IL_RGBA, Il.IL_UNSIGNED_BYTE) ' Convert every colour component into unsigned bytes
            'If your image contains alpha channel you can replace IL_RGB with IL_RGBA */
            Gl.glGenTextures(1, image_id)
            Gl.glEnable(Gl.GL_TEXTURE_2D)
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, image_id)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST_MIPMAP_LINEAR)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_GENERATE_MIPMAP, Gl.GL_TRUE)

            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT)

            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Il.ilGetInteger(Il.IL_IMAGE_BPP), Il.ilGetInteger(Il.IL_IMAGE_WIDTH), _
            Il.ilGetInteger(Il.IL_IMAGE_HEIGHT), 0, Il.ilGetInteger(Il.IL_IMAGE_FORMAT), Gl.GL_UNSIGNED_BYTE, _
            Il.ilGetData()) '  Texture specification 

            Gl.glFinish()

        End If
        Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0)
        Il.ilBindImage(0)
        Ilu.iluDeleteImage(texID)
        Return image_id
    End Function


    Public Function get_tank_image(ByVal ms As MemoryStream, ByVal index As Integer, ByVal make_id As Boolean) As Bitmap
        'Dim s As String = ""
        's = Gl.glGetError
        Dim image_id As Integer = -1
        'Dim app_local As String = Application.StartupPath.ToString

        Dim texID As UInt32
        Dim textIn(ms.Length) As Byte
        ms.Position = 0
        ms.Read(textIn, 0, ms.Length)
        texID = Ilu.iluGenImage() ' /* Generation of one image name */
        Il.ilBindImage(texID) '; /* Binding of image name */
        Dim success = Il.ilGetError
        Il.ilLoadL(Il.IL_PNG, textIn, textIn.Length)
        success = Il.ilGetError
        If success = Il.IL_NO_ERROR Then
            'Ilu.iluFlipImage()
            'Ilu.iluMirror()
            Dim width As Integer = Il.ilGetInteger(Il.IL_IMAGE_WIDTH)
            Dim height As Integer = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT)

            ' Create the bitmap.
            Dim Bitmapi = New System.Drawing.Bitmap(width, height, PixelFormat.Format32bppArgb)
            Dim rect As Rectangle = New Rectangle(0, 0, width, height)

            ' Store the DevIL image data into the bitmap.
            Dim bitmapData As BitmapData = Bitmapi.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb)

            Il.ilConvertImage(Il.IL_BGRA, Il.IL_UNSIGNED_BYTE)
            Il.ilCopyPixels(0, 0, 0, width, height, 1, Il.IL_BGRA, Il.IL_UNSIGNED_BYTE, bitmapData.Scan0)
            Bitmapi.UnlockBits(bitmapData)

            'If your image contains alpha channel you can replace IL_RGB with IL_RGBA */
            If make_id Then

                Gl.glGenTextures(1, image_id)
                Gl.glEnable(Gl.GL_TEXTURE_2D)
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, image_id)
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR)
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST_MIPMAP_LINEAR)
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_GENERATE_MIPMAP, Gl.GL_TRUE)

                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT)
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT)

                Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Il.ilGetInteger(Il.IL_IMAGE_BPP), Il.ilGetInteger(Il.IL_IMAGE_WIDTH), _
                                Il.ilGetInteger(Il.IL_IMAGE_HEIGHT), 0, Il.ilGetInteger(Il.IL_IMAGE_FORMAT), Gl.GL_UNSIGNED_BYTE, _
                                Il.ilGetData()) '  Texture specification 
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0)
                Il.ilBindImage(0)
                'ilu.iludeleteimage(texID)
                ReDim Preserve map_texture_ids(index + 1)
                map_texture_ids(index) = image_id
            End If

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0)
            Il.ilBindImage(0)
            Ilu.iluDeleteImage(texID)
            'GC.Collect()
            Return Bitmapi
        Else
            Stop
        End If
        Return Nothing
    End Function
    Public Function load_png(ByVal ms As MemoryStream) As Integer
        'Dim s As String = ""
        's = Gl.glGetError
        Dim image_id As Integer = -1
        'Dim app_local As String = Application.StartupPath.ToString

        Dim texID As UInt32
        Dim textIn(ms.Length) As Byte
        ms.Position = 0
        ms.Read(textIn, 0, ms.Length)
        texID = Ilu.iluGenImage() ' /* Generation of one image name */
        Il.ilBindImage(texID) '; /* Binding of image name */
        Dim success = Il.ilGetError
        Il.ilLoadL(Il.IL_PNG, textIn, textIn.Length)
        success = Il.ilGetError
        If success = Il.IL_NO_ERROR Then
            'Ilu.iluFlipImage()
            Ilu.iluMirror()
            Dim width As Integer = Il.ilGetInteger(Il.IL_IMAGE_WIDTH)
            Dim height As Integer = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT)


            Il.ilConvertImage(Il.IL_BGRA, Il.IL_UNSIGNED_BYTE)

            success = Il.ilConvertImage(Il.IL_RGBA, Il.IL_UNSIGNED_BYTE) ' Convert every colour component into unsigned bytes
            'If your image contains alpha channel you can replace IL_RGB with IL_RGBA */
            Gl.glGenTextures(1, image_id)
            Gl.glEnable(Gl.GL_TEXTURE_2D)
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, image_id)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR)

            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT)

            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Il.ilGetInteger(Il.IL_IMAGE_BPP), Il.ilGetInteger(Il.IL_IMAGE_WIDTH), _
            Il.ilGetInteger(Il.IL_IMAGE_HEIGHT), 0, Il.ilGetInteger(Il.IL_IMAGE_FORMAT), Gl.GL_UNSIGNED_BYTE, _
            Il.ilGetData()) '  Texture specification 
            'Gl.glGenerateMipmapEXT(Gl.GL_TEXTURE_2D)
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0)
            Il.ilBindImage(0)
            Ilu.iluDeleteImage(texID)
            Return image_id
        Else
            Stop
        End If
        Return Nothing
    End Function

    Public Function load_png_file(ByVal fs As String) As Integer
        'Dim s As String = ""
        's = Gl.glGetError
        Dim image_id As Integer = -1
        'Dim app_local As String = Application.StartupPath.ToString

        Dim texID As UInt32
        texID = Ilu.iluGenImage() ' /* Generation of one image name */
        Il.ilBindImage(texID) '; /* Binding of image name */
        Dim success = Il.ilGetError
        Il.ilLoad(Il.IL_PNG, fs)
        success = Il.ilGetError
        If success = Il.IL_NO_ERROR Then
            'Ilu.iluFlipImage()
            Ilu.iluMirror()
            Dim width As Integer = Il.ilGetInteger(Il.IL_IMAGE_WIDTH)
            Dim height As Integer = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT)


            Il.ilConvertImage(Il.IL_BGRA, Il.IL_UNSIGNED_BYTE)

            success = Il.ilConvertImage(Il.IL_RGBA, Il.IL_UNSIGNED_BYTE) ' Convert every colour component into unsigned bytes
            'If your image contains alpha channel you can replace IL_RGB with IL_RGBA */
            Gl.glGenTextures(1, image_id)
            Gl.glEnable(Gl.GL_TEXTURE_2D)
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, image_id)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR)

            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT)

            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Il.ilGetInteger(Il.IL_IMAGE_BPP), Il.ilGetInteger(Il.IL_IMAGE_WIDTH), _
            Il.ilGetInteger(Il.IL_IMAGE_HEIGHT), 0, Il.ilGetInteger(Il.IL_IMAGE_FORMAT), Gl.GL_UNSIGNED_BYTE, _
            Il.ilGetData()) '  Texture specification 
            'Gl.glGenerateMipmapEXT(Gl.GL_TEXTURE_2D)
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0)
            Il.ilBindImage(0)
            Ilu.iluDeleteImage(texID)
            Return image_id
        Else
            Stop
        End If
        Return Nothing
    End Function
    Public Function load_png_MIPS_file(ByVal fs As String) As Integer
        'Dim s As String = ""
        's = Gl.glGetError
        Dim image_id As Integer = -1
        'Dim app_local As String = Application.StartupPath.ToString

        Dim texID As UInt32
        texID = Ilu.iluGenImage() ' /* Generation of one image name */
        Il.ilBindImage(texID) '; /* Binding of image name */
        Dim success = Il.ilGetError
        Il.ilLoad(Il.IL_PNG, fs)
        success = Il.ilGetError
        If success = Il.IL_NO_ERROR Then
            'Ilu.iluFlipImage()
            Ilu.iluMirror()
            Dim width As Integer = Il.ilGetInteger(Il.IL_IMAGE_WIDTH)
            Dim height As Integer = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT)


            Il.ilConvertImage(Il.IL_BGRA, Il.IL_UNSIGNED_BYTE)

            success = Il.ilConvertImage(Il.IL_RGBA, Il.IL_UNSIGNED_BYTE) ' Convert every colour component into unsigned bytes
            'If your image contains alpha channel you can replace IL_RGB with IL_RGBA */
            Gl.glGenTextures(1, image_id)
            Gl.glEnable(Gl.GL_TEXTURE_2D)
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, image_id)

            If largestAnsio > 0 Then
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAX_ANISOTROPY_EXT, largestAnsio)
            End If

            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST_MIPMAP_LINEAR)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_GENERATE_MIPMAP, Gl.GL_TRUE)

            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT)

            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Il.ilGetInteger(Il.IL_IMAGE_BPP), Il.ilGetInteger(Il.IL_IMAGE_WIDTH), _
            Il.ilGetInteger(Il.IL_IMAGE_HEIGHT), 0, Il.ilGetInteger(Il.IL_IMAGE_FORMAT), Gl.GL_UNSIGNED_BYTE, _
            Il.ilGetData()) '  Texture specification 
            'Gl.glGenerateMipmapEXT(Gl.GL_TEXTURE_2D)
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0)
            Il.ilBindImage(0)
            Ilu.iluDeleteImage(texID)
            Return image_id
        Else
            Stop
        End If
        Return Nothing
    End Function

    Public Function Make_Depth_texture(size As Integer) As Integer
        'use for shadow mapping opjects
        'creates an empty texture ready for writing to
        Dim Id As Integer
        '1st texture target
        Gl.glGenTextures(1, Id)
        Gl.glBindTexture(Gl.GL_TEXTURE_2D, Id)
        Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST)
        Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST)
        Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_CLAMP_TO_EDGE)
        Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_CLAMP_TO_EDGE)

        Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB32F_ARB, size, size, 0, Gl.GL_RGBA, Gl.GL_FLOAT, Nothing)
        'Gl.glGenerateMipmapEXT(Gl.GL_TEXTURE_2D)
        Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0)
        Dim e = Gl.glGetError
        Return Id

    End Function
    Public Sub get_mini_map(ByVal ms As MemoryStream)
        Dim s As String = ""
        s = Gl.glGetError

        Dim app_local As String = Application.StartupPath.ToString

        Dim texID As UInt32
        ms.Position = 0
        Dim textIn(ms.Length) As Byte
        ms.Read(textIn, 0, ms.Length)
        texID = Ilu.iluGenImage() ' /* Generation of one image name */
        Il.ilBindImage(texID) '; /* Binding of image name */
        Dim success = Il.ilGetError
        Il.ilLoadL(Il.IL_DDS, textIn, textIn.Length)
        success = Il.ilGetError
        If success = Il.IL_NO_ERROR Then
            'Ilu.iluFlipImage()
            'Ilu.iluMirror()
            'Ilu.iluScale(256, 256, 1)
            Dim width As Integer = Il.ilGetInteger(Il.IL_IMAGE_WIDTH)
            Dim height As Integer = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT)

            ' Create the bitmap.
            Dim minimap = New System.Drawing.Bitmap(width, height, PixelFormat.Format32bppArgb)
            Dim rect As Rectangle = New Rectangle(0, 0, width, height)

            ' Store the DevIL image data into the bitmap.
            Dim bitmapData As BitmapData = minimap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb)

            Il.ilConvertImage(Il.IL_BGRA, Il.IL_UNSIGNED_BYTE)
            Il.ilCopyPixels(0, 0, 0, width, height, 1, Il.IL_BGRA, Il.IL_UNSIGNED_BYTE, bitmapData.Scan0)
            minimap.UnlockBits(bitmapData)
            Application.DoEvents()
            minimap.Dispose()
        Else
            Stop
        End If
        'br.Close()
        ms.Close()
        ms.Dispose()
        GC.Collect()
    End Sub

    Public Function get_basic_texture(ByVal ms As MemoryStream) As Integer
        'Dim s As String = ""
        's = Gl.glGetError
        Dim image_id As Integer = -1
        Dim success = Il.ilGetError
        'Dim app_local As String = Application.StartupPath.ToString

        Dim texID As UInt32
        Dim textIn(ms.Length) As Byte
        ms.Position = 0
        ms.Read(textIn, 0, ms.Length)
        texID = Ilu.iluGenImage()
        Il.ilBindImage(texID)
        Il.ilLoadL(Il.IL_DDS, textIn, textIn.Length)
        success = Il.ilGetError
        If success = Il.IL_NO_ERROR Then
            'Ilu.iluFlipImage()
            Ilu.iluMirror()
            Dim width As Integer = Il.ilGetInteger(Il.IL_IMAGE_WIDTH)
            Dim height As Integer = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT)


            Gl.glGenTextures(1, image_id)
            Gl.glEnable(Gl.GL_TEXTURE_2D)
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, image_id)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST_MIPMAP_LINEAR)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_GENERATE_MIPMAP, Gl.GL_TRUE)

            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT)
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT)

            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Il.ilGetInteger(Il.IL_IMAGE_BPP), Il.ilGetInteger(Il.IL_IMAGE_WIDTH), _
                            Il.ilGetInteger(Il.IL_IMAGE_HEIGHT), 0, Il.ilGetInteger(Il.IL_IMAGE_FORMAT), Gl.GL_UNSIGNED_BYTE, _
                            Il.ilGetData()) '  Texture specification 
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0)
            Il.ilBindImage(0)
            'ilu.iludeleteimage(texID)
            Il.ilBindImage(0)
            Ilu.iluDeleteImage(texID)
            Return image_id
        End If

        Return Nothing
    End Function

End Module
