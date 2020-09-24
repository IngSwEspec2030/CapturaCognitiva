import 'dart:io';
import 'dart:convert';
// import 'package:flutter_native_image/flutter_native_image.dart';
// import 'package:image/image.dart' as img;
import 'package:capturacognitivajaveriana/app/data/services/database.dart';

class CapturaService extends Database {
  Future<dynamic> analyzerImage(String userToken, File foto) async {
    final Uri uri = Uri.https(baseUrl, "api/Image/AnalyzerImage");

    var bytes = await foto.readAsBytes();

    // img.Image imgTemp = img.decodeImage(bytes);

    // img.Image resizedImg = img.copyResize(
    //   imgTemp,
    //   width: 1199,
    //   height: 1280,
    //   interpolation: img.Interpolation.linear
    // );

    print(base64.encode(bytes));
    print(base64Encode(bytes));

    var response = await httpClient.post(
      uri,
      headers: headers,
      body: jsonEncode({
        "UserToken": userToken, 
        "ImageBase64": base64.encode(bytes)
      }),
    );
    final data = json.decode(response.body);
    return data;
  }
}
