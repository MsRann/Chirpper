//	Copyright (c) 2012 Calvin Rien
//        http://the.darktable.com
//
//	This software is provided 'as-is', without any express or implied warranty. In
//	no event will the authors be held liable for any damages arising from the use
//	of this software.
//
//	Permission is granted to anyone to use this software for any purpose,
//	including commercial applications, and to alter it and redistribute it freely,
//	subject to the following restrictions:
//
//	1. The origin of this software must not be misrepresented; you must not claim
//	that you wrote the original software. If you use this software in a product,
//	an acknowledgment in the product documentation would be appreciated but is not
//	required.
//
//	2. Altered source versions must be plainly marked as such, and must not be
//	misrepresented as being the original software.
//
//	3. This notice may not be removed or altered from any source distribution.
//
//  =============================================================================
//
//  derived from Gregorio Zanon's script
//  http://forum.unity3d.com/threads/119295-Writing-AudioListener.GetOutputData-to-wav-problem?p=806734&viewfull=1#post806734
 
using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
 
public static class SavWav {
 
	const int HEADER_SIZE = 44;
	public static float test = 0.4f;
	public static byte[] Save(AudioClip clip) {
//		if (!filename.ToLower().EndsWith(".wav")) {
//			filename += ".wav";
//		}
 
		//var filepath = Path.Combine(Application.persistentDataPath, filename);
		//var filepath = Path.Combine(Application.dataPath, filename);
 
		//Debug.Log(filepath);
 
		// Make sure directory exists if user is saving to sub dir.
		//Directory.CreateDirectory(Path.GetDirectoryName(filepath));
 
		//using (var fileStream = CreateEmpty(filepath)) {

//			AudioClip ac = clip;
//			float lengthL = ac.length;
//			float samplesL = ac.samples;
//			float samplesPerSec = (float)samplesL/lengthL;
//			float[] samples = new float[(int)(samplesPerSec * actualTime)];
//			ac.GetData(samples,0);
//			
//			clip = AudioClip.Create("RecordedSound",(int)(actualTime*samplesPerSec),1,44100,false,false);
//			Debug.Log("actualTime: " + actualTime + " Length now: " + (actualTime * samplesPerSec));
//			clip.SetData(samples,0);

			//TrimSilence (clip, test);

			byte[] file = ConvertAndWrite(clip);
 			
			byte[] header = WriteHeader(clip,file.Length);
			byte[] final = new byte[header.Length + file.Length];
			Buffer.BlockCopy(header,0,final,0,header.Length);
			Buffer.BlockCopy(file,0,final,header.Length,file.Length);
			return final;
		//}
 
		return new byte[0]; // TODO: return false if there's a failure saving the file
	}
 
	public static AudioClip TrimSilence(AudioClip clip, float min) {
		var samples = new float[clip.samples];
 
		clip.GetData(samples, 0);
 
		return TrimSilence(new List<float>(samples), min, clip.channels, clip.frequency);
	}
 
	public static AudioClip TrimSilence(List<float> samples, float min, int channels, int hz) {
		return TrimSilence(samples, min, channels, hz, false, false);
	}
 
	public static AudioClip TrimSilence(List<float> samples, float min, int channels, int hz, bool _3D, bool stream) {
		int i;
 
		for (i=0; i<samples.Count; i++) {
			if (Mathf.Abs(samples[i]) > min) {
				break;
			}
		}
 
		samples.RemoveRange(0, i);
 
		for (i=samples.Count - 1; i>0; i--) {
			if (Mathf.Abs(samples[i]) > min) {
				break;
			}
		}
 
		samples.RemoveRange(i, samples.Count - i);
 
		var clip = AudioClip.Create("TempClip", samples.Count, channels, hz, _3D, stream);
 
		clip.SetData(samples.ToArray(), 0);
 
		return clip;
	}
 
	static FileStream CreateEmpty(string filepath) {
		var fileStream = new FileStream(filepath, FileMode.Create);
	    byte emptyByte = new byte();
 
	    for(int i = 0; i < HEADER_SIZE; i++) //preparing the header
	    {
	        fileStream.WriteByte(emptyByte);
	    }
 
		return fileStream;
	}
 

	static byte[] ConvertAndWrite(AudioClip clip) {

		var samples = new float[clip.samples];
 
		clip.GetData(samples, 0);
 
		Int16[] intData = new Int16[samples.Length];
		//converting in 2 float[] steps to Int16[], //then Int16[] to Byte[]
 
		Byte[] bytesData = new Byte[samples.Length * 2];
		//bytesData array is twice the size of
		//dataSource array because a float converted in Int16 is 2 bytes.
 
		int rescaleFactor = 32767; //to convert float to Int16
 
		for (int i = 0; i<samples.Length; i++) {
			intData[i] = (short) (samples[i] * rescaleFactor);
			Byte[] byteArr = new Byte[2];
			byteArr = BitConverter.GetBytes(intData[i]);
			byteArr.CopyTo(bytesData, i * 2);
		}
		return bytesData;
		//fileStream.Write(bytesData, 0, bytesData.Length);
	}
 
	static Byte[] WriteHeader(AudioClip clip,int length) {
 
		var hz = clip.frequency;
		var channels = clip.channels;
		var samples = clip.samples;
 
	//	fileStream.Seek(0, SeekOrigin.Begin);

		byte[] fullHeader = new byte[44];

		Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
		//fileStream.Write(riff, 0, 4);
		Buffer.BlockCopy(riff,0,fullHeader,0,4);
	
		Byte[] chunkSize = BitConverter.GetBytes(length - 4);
		//fileStream.Write(chunkSize, 0, 4);
		Buffer.BlockCopy(chunkSize,0,fullHeader,4,4);
	
		Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
		//fileStream.Write(wave, 0, 4);
		Buffer.BlockCopy(wave,0,fullHeader,8,4);
	
		Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
		//fileStream.Write(fmt, 0, 4);
		Buffer.BlockCopy(fmt,0,fullHeader,12,4);

		Byte[] subChunk1 = BitConverter.GetBytes(16);
		//fileStream.Write(subChunk1, 0, 4);
		Buffer.BlockCopy(subChunk1,0,fullHeader,16,4);
 
		UInt16 two = 2;
		UInt16 one = 1;
 
		Byte[] audioFormat = BitConverter.GetBytes(one);
		//fileStream.Write(audioFormat, 0, 2);
		Buffer.BlockCopy (audioFormat, 0, fullHeader,20, 2);
 
		Byte[] numChannels = BitConverter.GetBytes(channels);
		//fileStream.Write(numChannels, 0, 2);
		Buffer.BlockCopy(numChannels,0,fullHeader,22,2);
 
		Byte[] sampleRate = BitConverter.GetBytes(hz);
		//fileStream.Write(sampleRate, 0, 4);
		Buffer.BlockCopy(sampleRate,0,fullHeader,24,4);
 
		Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2
		//fileStream.Write(byteRate, 0, 4);
		Buffer.BlockCopy(byteRate,0,fullHeader,28,4);
 
		UInt16 blockAlign = (ushort) (channels * 2);
		//fileStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);
		Buffer.BlockCopy(BitConverter.GetBytes(blockAlign),0,fullHeader,32,2);
 
		UInt16 bps = 16;
		Byte[] bitsPerSample = BitConverter.GetBytes(bps);
		//fileStream.Write(bitsPerSample, 0, 2);
		Buffer.BlockCopy(bitsPerSample,0,fullHeader,34,2);
 
		Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
		//fileStream.Write(datastring, 0, 4);
		Buffer.BlockCopy(datastring,0,fullHeader,36,4);
 
		Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);
		//fileStream.Write(subChunk2, 0, 4);
		Buffer.BlockCopy(subChunk2,0,fullHeader,40,4);

		return fullHeader;
		//print(fileStream.ToString ());
//		fileStream.Close();
	}
}