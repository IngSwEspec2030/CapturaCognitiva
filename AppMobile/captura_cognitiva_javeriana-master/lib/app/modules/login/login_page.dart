import 'dart:ui';

import 'package:capturacognitivajaveriana/app/data/repositories/local_storage.dart';
import 'package:capturacognitivajaveriana/app/routes/app_pages.dart';
import "package:get/get.dart";
import 'package:flutter/material.dart';
import 'package:capturacognitivajaveriana/app/data/services/auth_service.dart';

class LoginPage extends StatefulWidget {
  @override
  _LoginPageState createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  final AuthService authService = AuthService();
  final Prefs prefs = Prefs();
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  TextEditingController _emailController = TextEditingController();
  TextEditingController _passController = TextEditingController();
  bool loading = false;

  @override
  Widget build(BuildContext context) {
    final ThemeData theme = Theme.of(context);

    return SafeArea(
      child: Scaffold(
        backgroundColor: theme.primaryColor,
        appBar: AppBar(
          elevation: 0.0,
        ),
        body: Container(
          constraints: BoxConstraints.expand(),
          decoration: BoxDecoration(
              color: Colors.white,
              borderRadius: BorderRadius.only(
                topLeft: Radius.circular(20.0),
                topRight: Radius.circular(20.0),
              )),
          child: SingleChildScrollView(
            padding: EdgeInsets.all(12.0),
            child: Form(
              key: _formKey,
              child: Column(
                mainAxisSize: MainAxisSize.max,
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  SizedBox(
                    height: 20.0,
                  ),
                  Text(
                    "Inicia sesión",
                    style: Theme.of(context)
                        .textTheme
                        .headline5
                        .apply(fontWeightDelta: 900),
                  ),
                  ListTile(
                    leading: Icon(Icons.email),
                    title: TextFormField(
                      controller: _emailController,
                      keyboardType: TextInputType.emailAddress,
                      decoration:
                          InputDecoration(isDense: true, labelText: "Correo"),
                      validator: (value) {
                        if (GetUtils.isEmail(value)) {
                          return null;
                        }
                        return "Ingrese un correo válido";
                      },
                    ),
                  ),
                  ListTile(
                    leading: Icon(Icons.lock_outline),
                    title: TextFormField(
                      obscureText: true,
                      controller: _passController,
                      keyboardType: TextInputType.text,
                      decoration: InputDecoration(
                          isDense: true, labelText: "Contraseña"),
                      validator: (value) {
                        if (GetUtils.isLengthGreaterThan(value, 4)) {
                          return null;
                        }
                        return "Contraseña muy corta";
                      },
                    ),
                  ),
                  Container(
                    width: double.infinity,
                    child: FlatButton.icon(
                      icon: loading
                          ? SizedBox(
                              width: 20.0,
                              height: 20.0,
                              child: CircularProgressIndicator(
                                valueColor:
                                    AlwaysStoppedAnimation<Color>(Colors.white),
                                strokeWidth: 3.0,
                              ))
                          : SizedBox.shrink(),
                      color: theme.accentColor,
                      label: Text(
                        "INGRESAR",
                        style: TextStyle(fontWeight: FontWeight.bold),
                      ),
                      onPressed: () async {
                        if (_formKey.currentState.validate()) {
                          setState(() {
                            loading = true;
                          });
                          authService
                              .login(
                                  _emailController.text, _passController.text)
                              .then((resp) {
                                print(resp.toString());
                                if (resp["response"]) {
                                  setState(() {
                                    prefs.logged = true;
                                    prefs.usetToken = resp["userId"];
                                  });

                                  print("Logged: ${prefs.logged}");
                                  Get.offAndToNamed(Routes.HOME);
                                } else {
                                  Get.defaultDialog(
                                    title: "Error de auntenticación.",
                                    content: Container(
                                      child: Text(
                                        resp["message"],
                                      ),
                                    ),
                                  );
                                }
                              })
                              .catchError((onError) {
                                  Get.defaultDialog(
                                    title: "Error de auntenticación.",
                                    content: Container(
                                      child: Text(
                                        onError.toString(),
                                      ),
                                    ),
                                  );
                              });
                          setState(() {
                            loading = false;
                          });
                        }
                      },
                    ),
                  ),
                  Container(
                    width: double.infinity,
                    child: FlatButton(
                      color: Colors.blueGrey[200],
                      child: Text(
                        "CAMBIAR CONTRASEÑA",
                        style: TextStyle(fontWeight: FontWeight.bold),
                      ),
                      onPressed: () {
                        Get.toNamed(Routes.RESET);
                      },
                    ),
                  ),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}
