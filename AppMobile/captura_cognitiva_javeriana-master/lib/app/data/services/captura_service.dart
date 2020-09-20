import 'dart:io';
import 'dart:convert';
import 'package:image/image.dart' as img;
import 'package:capturacognitivajaveriana/app/data/services/database.dart';

const baseUrl = 'capturainstancia.azurewebsites.net';

class CapturaService extends Database {
  Future<dynamic> analyzerImage(String userToken, File foto) async {
    final Uri uri = Uri.https(baseUrl, "api/Image/AnalyzerImage");
    var soporte = await foto.readAsBytes();
    img.Image imgTemp = img.decodeImage(soporte);
    img.Image resizedImg = img.copyResize(imgTemp, width: 300);
    print(base64.encode(img.encodeJpg(resizedImg, quality: 100)));

    var response = await httpClient.post(
      uri,
      headers: headers,
      body: jsonEncode({
        "UserToken": userToken,
        "ImageBase64": base64.encode(img.encodeJpg(resizedImg, quality: 100))
      }),
    );
    final data = json.decode(response.body);
    return data;
  }
}
