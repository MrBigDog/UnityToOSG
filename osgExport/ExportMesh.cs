using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace nwTools
{

public class MeshExporter
{
    public static string ExportGeometry( ref SceneData sceneData, ref SceneMeshRenderer smr,
                                         ref SceneMesh mesh, string spaces )
    {
        string osgData = spaces + "useDisplayList TRUE\n"
                       + spaces + "useVertexBufferObjects FALSE\n";
        
        // Attach textures
        if ( smr!=null )
            osgData += MaterialExporter.ExportStateSet( ref sceneData, ref smr, spaces );
        
        // Add all primitive sets
        osgData += spaces + "PrimitiveSets " + mesh.subMeshCount + " {\n";
        for ( int i=0; i<mesh.subMeshCount; ++i )
        {
            int numElements = mesh.triangles[i].Length;
            osgData += spaces + "  DrawElements" + (numElements<65535 ? "UShort" : "UInt")
                     + " TRIANGLES " + numElements + " {\n";
            for ( int j=0; j<numElements; j+=3 )
            {
                osgData += spaces + "    " + mesh.triangles[i][j] + " "
                         + mesh.triangles[i][j+1] + " " + mesh.triangles[i][j+2] + "\n";
            }
            osgData += spaces + "  }\n";
        }
        osgData += spaces + "}\n";
        
        // Add all vertices
        osgData += spaces + "VertexArray Vec3Array " + mesh.vertexCount + " {\n";
        for ( int i=0; i<mesh.vertexCount; ++i )
        {
            Vector3 v = mesh.vertexPositions[i];
            osgData += spaces + "  " + v.x + " " + v.y + " " + v.z + "\n";
        }
        osgData += spaces + "}\n";
        
        // Add all normals
        if ( mesh.vertexNormals.Length>0 )
        {
            osgData += spaces + "NormalBinding PER_VERTEX\n"
                     + spaces + "NormalArray Vec3Array " + mesh.vertexCount + " {\n";
            for ( int i=0; i<mesh.vertexCount; ++i )
            {
                Vector3 v = mesh.vertexNormals[i];
                osgData += spaces + "  " + v.x + " " + v.y + " " + v.z + "\n";
            }
            osgData += spaces + "}\n";
        }
        
        // Add all UVs
        if ( mesh.vertexUV.Length>0 )
        {
            osgData += spaces + "TexCoordArray 0 Vec2Array " + mesh.vertexCount + " {\n";
            for ( int i=0; i<mesh.vertexCount; ++i )
            {
                Vector2 v = mesh.vertexUV[i];
                osgData += spaces + "  " + v.x + " " + v.y + "\n";
            }
            osgData += spaces + "}\n";
        }
        
        if ( mesh.vertexUV2.Length>0 )
        {
            osgData += spaces + "TexCoordArray 1 Vec2Array " + mesh.vertexCount + " {\n";
            for ( int i=0; i<mesh.vertexCount; ++i )
            {
                Vector2 v = mesh.vertexUV2[i];
                osgData += spaces + "  " + v.x + " " + v.y + "\n";
            }
            osgData += spaces + "}\n";
        }
        
        // TODO: tangents & bones
        return osgData;
    }
    
    public static void Reset()
    {
    }
}

}
