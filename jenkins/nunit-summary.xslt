<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output indent="no" method='text'/>
<xsl:strip-space elements="*"/>
	
<xsl:template match="/">
	<xsl:apply-templates/>
</xsl:template>

<xsl:template match="//test-suite[@type='TestFixture']">
	<xsl:text>&#10;</xsl:text>
	<xsl:value-of select="@name"/>
	<xsl:text>&#10;</xsl:text>
	<xsl:for-each select="current()/results/test-case">
		<xsl:choose>
			<xsl:when test="@result">
				<xsl:if test="@result='Success'">
					<xsl:text>    [PASS] </xsl:text>
				</xsl:if>
				<xsl:if test="@result='Failure'">
					<xsl:text>    [FAIL] </xsl:text>
				</xsl:if>
				<xsl:if test="@result='Ignored'">
					<xsl:text>    [IGNORED] </xsl:text>
				</xsl:if>
				<xsl:if test="@result='Inconclusive'">
					<xsl:text>    [INCONCLUSIVE] </xsl:text>
				</xsl:if>
			</xsl:when>
		</xsl:choose>
		<xsl:value-of select="../../@name"/><xsl:text>.</xsl:text><xsl:value-of select="@name"/><xsl:text>&#10;</xsl:text>
	</xsl:for-each>
	<xsl:choose>
		<xsl:when test="@time">
			<xsl:value-of select="concat(@name,' : ', @time)"/>
		</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="@name"/>
		</xsl:otherwise>
	</xsl:choose>
	<xsl:text>&#10;</xsl:text>
</xsl:template>
</xsl:stylesheet>

  
