
EXTRA_DIST =  Makefile

# Warning: This is an automatically generated file, do not edit!

srcdir=.
top_srcdir=../..

include $(top_srcdir)/config.make

ifeq ($(CONFIG),DEBUG_X86)
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = bin/Debug/Valle.Distribuido.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Valle.SqlUtilidades/Valle.SqlUtilidades/bin/Debug/Valle.SqlUtilidades.dll \
	../../Valle.Utilidades/Valle.Utilidades/bin/Debug/Valle.Utilidades.dll
BUILD_DIR = bin/Debug

VALLE_SQLUTILIDADES_DLL_SOURCE=../../Valle.SqlUtilidades/Valle.SqlUtilidades/bin/Debug/Valle.SqlUtilidades.dll
VALLE_UTILIDADES_DLL_SOURCE=../../Valle.Utilidades/Valle.Utilidades/bin/Debug/Valle.Utilidades.dll
VALLE_UTILIDADES_DLL_MDB_SOURCE=../../Valle.Utilidades/Valle.Utilidades/bin/Debug/Valle.Utilidades.dll.mdb
VALLE_UTILIDADES_DLL_MDB=$(BUILD_DIR)/Valle.Utilidades.dll.mdb
MYSQL_DATA_DLL_SOURCE=../../Valle.SqlUtilidades/MySql.Data.dll
VALLE_SQLUTILIDADES_DLL_MDB_SOURCE=../../Valle.SqlUtilidades/Valle.SqlUtilidades/bin/Debug/Valle.SqlUtilidades.dll.mdb
VALLE_SQLUTILIDADES_DLL_MDB=$(BUILD_DIR)/Valle.SqlUtilidades.dll.mdb
VALLE_DISTRIBUIDO_DLL_MDB_SOURCE=bin/Debug/Valle.Distribuido.dll.mdb
VALLE_DISTRIBUIDO_DLL_MDB=$(BUILD_DIR)/Valle.Distribuido.dll.mdb

endif

ifeq ($(CONFIG),RELEASE_X86)
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize-
ASSEMBLY = bin/Release/Valle.Distribuido.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Valle.SqlUtilidades/Valle.SqlUtilidades/bin/Release/Valle.SqlUtilidades.dll \
	../../Valle.Utilidades/Valle.Utilidades/bin/Release/Valle.Utilidades.dll
BUILD_DIR = bin/Release

VALLE_SQLUTILIDADES_DLL_SOURCE=../../Valle.SqlUtilidades/Valle.SqlUtilidades/bin/Release/Valle.SqlUtilidades.dll
VALLE_UTILIDADES_DLL_SOURCE=../../Valle.Utilidades/Valle.Utilidades/bin/Release/Valle.Utilidades.dll
VALLE_UTILIDADES_DLL_MDB=
MYSQL_DATA_DLL_SOURCE=../../Valle.SqlUtilidades/MySql.Data.dll
VALLE_SQLUTILIDADES_DLL_MDB=
VALLE_DISTRIBUIDO_DLL_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(VALLE_SQLUTILIDADES_DLL) \
	$(VALLE_UTILIDADES_DLL) \
	$(VALLE_UTILIDADES_DLL_MDB) \
	$(MYSQL_DATA_DLL) \
	$(VALLE_SQLUTILIDADES_DLL_MDB) \
	$(VALLE_DISTRIBUIDO_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(VALLE_DISTRIBUIDO_PC)  


RESGEN=resgen2

VALLE_SQLUTILIDADES_DLL = $(BUILD_DIR)/Valle.SqlUtilidades.dll
VALLE_UTILIDADES_DLL = $(BUILD_DIR)/Valle.Utilidades.dll
MYSQL_DATA_DLL = $(BUILD_DIR)/MySql.Data.dll
VALLE_DISTRIBUIDO_PC = $(BUILD_DIR)/valle.distribuido.pc

FILES = \
	AssemblyInfo.cs \
	GesMensajes.cs \
	GesRemFicheros.cs \
	Socket/ClienteSock.cs \
	Socket/ServidorSock.cs \
	Socket/UtilMiSock.cs \
	SQLMixto/IMensajesDec.cs \
	SQLRemoting/SqlRemoting.cs \
	SQLRemoting/RegistrarServidor.cs \
	SQLSocket/SqlSock.cs \
	SQLSocket/RegistrarServidores.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Socket \
	SQLMixto \
	SQLRemoting \
	SQLSocket \
	FicherosRemoting \
	valle.distribuido.pc.in 

REFERENCES =  \
	System \
	System.Data \
	System.Drawing \
	System.Drawing.Design \
	System.Runtime.Remoting \
	System.Xml

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

#Targets
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG)  $(top_srcdir)/config.make

include $(top_srcdir)/Makefile.include
#include $(srcdir)/custom-hooks.make



$(eval $(call emit-deploy-target,VALLE_SQLUTILIDADES_DLL))
$(eval $(call emit-deploy-target,VALLE_UTILIDADES_DLL))
$(eval $(call emit-deploy-target,VALLE_UTILIDADES_DLL_MDB))
$(eval $(call emit-deploy-target,MYSQL_DATA_DLL))
$(eval $(call emit-deploy-target,VALLE_SQLUTILIDADES_DLL_MDB))
$(eval $(call emit-deploy-wrapper,VALLE_DISTRIBUIDO_PC,valle.distribuido.pc))


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
	$(call cp,$(VALLE_SQLUTILIDADES_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(VALLE_UTILIDADES_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(VALLE_UTILIDADES_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(MYSQL_DATA_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(VALLE_SQLUTILIDADES_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(VALLE_DISTRIBUIDO_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	mkdir -p '$(DESTDIR)$(libdir)/pkgconfig'
	$(call cp,$(VALLE_DISTRIBUIDO_PC),$(DESTDIR)$(libdir)/pkgconfig)
	make post-install-local-hook prefix=$(prefix)

uninstall-local: $(ASSEMBLY) $(ASSEMBLY_MDB)
	make pre-uninstall-local-hook prefix=$(prefix)
	make uninstall-satellite-assemblies prefix=$(prefix)
	$(call rm,$(ASSEMBLY),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(ASSEMBLY_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(VALLE_SQLUTILIDADES_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(VALLE_UTILIDADES_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(VALLE_UTILIDADES_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(MYSQL_DATA_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(VALLE_SQLUTILIDADES_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(VALLE_DISTRIBUIDO_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(VALLE_DISTRIBUIDO_PC),$(DESTDIR)$(libdir)/pkgconfig)
	make post-uninstall-local-hook prefix=$(prefix)

install: install-local
uninstall: uninstall-local
clean: clean-local

include $(top_srcdir)/rules.make
