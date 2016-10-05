
EXTRA_DIST =  Makefile

# Warning: This is an automatically generated file, do not edit!

srcdir=.
top_srcdir=..

include $(top_srcdir)/config.make

ifeq ($(CONFIG),DEBUG_X86)
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = bin/Debug/ToolsTpv.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../Valle.Distribuido/Valle.Distribuido/bin/Debug/Valle.Distribuido.dll \
	../Valle.Utilidades/Valle.Utilidades/bin/Debug/Valle.Utilidades.dll \
	../Valle.SqlUtilidades/Valle.SqlUtilidades/bin/Debug/Valle.SqlUtilidades.dll \
	../Valle.Seguridad/bin/Debug/Valle.Seguridad.dll
BUILD_DIR = bin/Debug

VALLE_DISTRIBUIDO_DLL_SOURCE=../Valle.Distribuido/Valle.Distribuido/bin/Debug/Valle.Distribuido.dll
VALLE_SQLUTILIDADES_DLL_SOURCE=../Valle.SqlUtilidades/Valle.SqlUtilidades/bin/Debug/Valle.SqlUtilidades.dll
VALLE_UTILIDADES_DLL_SOURCE=../Valle.Utilidades/Valle.Utilidades/bin/Debug/Valle.Utilidades.dll
VALLE_UTILIDADES_DLL_MDB_SOURCE=../Valle.Utilidades/Valle.Utilidades/bin/Debug/Valle.Utilidades.dll.mdb
VALLE_UTILIDADES_DLL_MDB=$(BUILD_DIR)/Valle.Utilidades.dll.mdb
MYSQL_DATA_DLL_SOURCE=../Valle.SqlUtilidades/MySql.Data.dll
VALLE_SQLUTILIDADES_DLL_MDB_SOURCE=../Valle.SqlUtilidades/Valle.SqlUtilidades/bin/Debug/Valle.SqlUtilidades.dll.mdb
VALLE_SQLUTILIDADES_DLL_MDB=$(BUILD_DIR)/Valle.SqlUtilidades.dll.mdb
VALLE_DISTRIBUIDO_DLL_MDB_SOURCE=../Valle.Distribuido/Valle.Distribuido/bin/Debug/Valle.Distribuido.dll.mdb
VALLE_DISTRIBUIDO_DLL_MDB=$(BUILD_DIR)/Valle.Distribuido.dll.mdb
VALLE_SEGURIDAD_DLL_SOURCE=../Valle.Seguridad/bin/Debug/Valle.Seguridad.dll
VALLE_SEGURIDAD_DLL_MDB_SOURCE=../Valle.Seguridad/bin/Debug/Valle.Seguridad.dll.mdb
VALLE_SEGURIDAD_DLL_MDB=$(BUILD_DIR)/Valle.Seguridad.dll.mdb
TOOLSTPV_DLL_MDB_SOURCE=bin/Debug/ToolsTpv.dll.mdb
TOOLSTPV_DLL_MDB=$(BUILD_DIR)/ToolsTpv.dll.mdb

endif

ifeq ($(CONFIG),RELEASE_X86)
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize-
ASSEMBLY = bin/Release/ToolsTpv.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../Valle.Distribuido/Valle.Distribuido/bin/Release/Valle.Distribuido.dll \
	../Valle.Utilidades/Valle.Utilidades/bin/Release/Valle.Utilidades.dll \
	../Valle.SqlUtilidades/Valle.SqlUtilidades/bin/Release/Valle.SqlUtilidades.dll \
	../Valle.Seguridad/bin/Release/Valle.Seguridad.dll
BUILD_DIR = bin/Release

VALLE_DISTRIBUIDO_DLL_SOURCE=../Valle.Distribuido/Valle.Distribuido/bin/Release/Valle.Distribuido.dll
VALLE_SQLUTILIDADES_DLL_SOURCE=../Valle.SqlUtilidades/Valle.SqlUtilidades/bin/Release/Valle.SqlUtilidades.dll
VALLE_UTILIDADES_DLL_SOURCE=../Valle.Utilidades/Valle.Utilidades/bin/Release/Valle.Utilidades.dll
VALLE_UTILIDADES_DLL_MDB=
MYSQL_DATA_DLL_SOURCE=../Valle.SqlUtilidades/MySql.Data.dll
VALLE_SQLUTILIDADES_DLL_MDB=
VALLE_DISTRIBUIDO_DLL_MDB=
VALLE_SEGURIDAD_DLL_SOURCE=../Valle.Seguridad/bin/Release/Valle.Seguridad.dll
VALLE_SEGURIDAD_DLL_MDB=
TOOLSTPV_DLL_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(VALLE_DISTRIBUIDO_DLL) \
	$(VALLE_SQLUTILIDADES_DLL) \
	$(VALLE_UTILIDADES_DLL) \
	$(VALLE_UTILIDADES_DLL_MDB) \
	$(MYSQL_DATA_DLL) \
	$(VALLE_SQLUTILIDADES_DLL_MDB) \
	$(VALLE_DISTRIBUIDO_DLL_MDB) \
	$(VALLE_SEGURIDAD_DLL) \
	$(VALLE_SEGURIDAD_DLL_MDB) \
	$(TOOLSTPV_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(VALLE_TOOLSTPV_PC)  


RESGEN=resgen2

VALLE_DISTRIBUIDO_DLL = $(BUILD_DIR)/Valle.Distribuido.dll
VALLE_SQLUTILIDADES_DLL = $(BUILD_DIR)/Valle.SqlUtilidades.dll
VALLE_UTILIDADES_DLL = $(BUILD_DIR)/Valle.Utilidades.dll
MYSQL_DATA_DLL = $(BUILD_DIR)/MySql.Data.dll
VALLE_SEGURIDAD_DLL = $(BUILD_DIR)/Valle.Seguridad.dll
VALLE_TOOLSTPV_PC = $(BUILD_DIR)/valle.toolstpv.pc

FILES = \
	AssemblyInfo.cs \
	IDPrometedor.cs \
	Articulo.cs \
	CrearBaseDatos.cs \
	GeneradorDeArticulos.cs \
	GuardarTicket.cs \
	GesAlmacen.cs \
	GesCamareros.cs \
	GesMesas.cs \
	GesMesasRem.cs \
	GesSincronizar.cs \
	GesVentas.cs \
	RegistrarServidor.cs \
	ControlImpresora.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	valle.toolstpv.pc.in 

REFERENCES =  \
	System \
	System.Data \
	System.Drawing \
	System.Runtime.Remoting \
	System.Runtime.Serialization \
	System.Runtime.Serialization.Formatters.Soap

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

#Targets
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG)  $(top_srcdir)/config.make

include $(top_srcdir)/Makefile.include
#include $(srcdir)/custom-hooks.make



$(eval $(call emit-deploy-target,VALLE_DISTRIBUIDO_DLL))
$(eval $(call emit-deploy-target,VALLE_SQLUTILIDADES_DLL))
$(eval $(call emit-deploy-target,VALLE_UTILIDADES_DLL))
$(eval $(call emit-deploy-target,VALLE_UTILIDADES_DLL_MDB))
$(eval $(call emit-deploy-target,MYSQL_DATA_DLL))
$(eval $(call emit-deploy-target,VALLE_SQLUTILIDADES_DLL_MDB))
$(eval $(call emit-deploy-target,VALLE_DISTRIBUIDO_DLL_MDB))
$(eval $(call emit-deploy-target,VALLE_SEGURIDAD_DLL))
$(eval $(call emit-deploy-target,VALLE_SEGURIDAD_DLL_MDB))
$(eval $(call emit-deploy-wrapper,VALLE_TOOLSTPV_PC,valle.toolstpv.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'


$(ASSEMBLY_MDB): $(ASSEMBLY)
$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	make pre-all-local-hook prefix=$(prefix)
	mkdir -p $(shell dirname $(ASSEMBLY))
	make $(CONFIG)_BeforeBuild
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
	make $(CONFIG)_AfterBuild
	make post-all-local-hook prefix=$(prefix)

install-local: $(ASSEMBLY) $(ASSEMBLY_MDB)
	make pre-install-local-hook prefix=$(prefix)
	make install-satellite-assemblies prefix=$(prefix)
	mkdir -p '$(DESTDIR)$(libdir)/$(PACKAGE)'
	$(call cp,$(ASSEMBLY),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(ASSEMBLY_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(VALLE_DISTRIBUIDO_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(VALLE_SQLUTILIDADES_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(VALLE_UTILIDADES_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(VALLE_UTILIDADES_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(MYSQL_DATA_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(VALLE_SQLUTILIDADES_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(VALLE_DISTRIBUIDO_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(VALLE_SEGURIDAD_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(VALLE_SEGURIDAD_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(TOOLSTPV_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	mkdir -p '$(DESTDIR)$(libdir)/pkgconfig'
	$(call cp,$(VALLE_TOOLSTPV_PC),$(DESTDIR)$(libdir)/pkgconfig)
	make post-install-local-hook prefix=$(prefix)

uninstall-local: $(ASSEMBLY) $(ASSEMBLY_MDB)
	make pre-uninstall-local-hook prefix=$(prefix)
	make uninstall-satellite-assemblies prefix=$(prefix)
	$(call rm,$(ASSEMBLY),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(ASSEMBLY_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(VALLE_DISTRIBUIDO_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(VALLE_SQLUTILIDADES_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(VALLE_UTILIDADES_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(VALLE_UTILIDADES_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(MYSQL_DATA_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(VALLE_SQLUTILIDADES_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(VALLE_DISTRIBUIDO_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(VALLE_SEGURIDAD_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(VALLE_SEGURIDAD_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(TOOLSTPV_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(VALLE_TOOLSTPV_PC),$(DESTDIR)$(libdir)/pkgconfig)
	make post-uninstall-local-hook prefix=$(prefix)

install: install-local
uninstall: uninstall-local
clean: clean-local

include $(top_srcdir)/rules.make
