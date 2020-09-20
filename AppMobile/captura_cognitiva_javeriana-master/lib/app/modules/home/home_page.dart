import 'package:flutter/material.dart';
import 'package:get/route_manager.dart';
import 'package:capturacognitivajaveriana/app/routes/app_pages.dart';
import 'package:capturacognitivajaveriana/app/data/repositories/local_storage.dart';

class HomePage extends StatelessWidget {
  final Prefs prefs = Prefs();
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Color(0xfffafafa),
      appBar: AppBar(title: Text("Captura cognitiva"), actions: [
        IconButton(
          icon: Icon(Icons.exit_to_app),
          onPressed: () {
            prefs.logged = false;

            if (!prefs.logged) {
              prefs.usetToken = "";
              Get.offAndToNamed(Routes.SPLASH);
            }
          },
        )
      ]),
      body: Container(
        color: Color(0xffFAFAFA),
        constraints: BoxConstraints.expand(),
        padding: EdgeInsets.all(12.0),
        child: Column(
          children: [
            Material(
              elevation: 2.0,
              color: Colors.white,
              child: ListTile(
                leading: Icon(Icons.camera_alt),
                title: Text("Capturar imagen"),
                onTap: () {
                  Get.toNamed(Routes.CAPTURA);
                },
                trailing: Icon(Icons.keyboard_arrow_right),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
